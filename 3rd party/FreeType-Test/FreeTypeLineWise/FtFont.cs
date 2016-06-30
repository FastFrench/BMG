using System;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using FreeTypeWrap;

namespace FreeType
{

	/// <summary>The font class for 2D text printing in a OpenGL scene.</summary>
	/// <remarks>Based on open-source Font3D class, e. g. presented in: http://www.gamedev.ru/code/forum/?id=192978 and adopted to C# and
	/// AltSketch.AltNETType library in: http://www.codeproject.com/Tips/668161/Rendering-AltNETType-equals-NET-FreeType-port-with.</remarks>
	public class FtFont : IDisposable
	{

		///<summary>The geometry of a glyph within the -all-glyphs- texture.</summary>
		struct GlyphGeometry
		{
			public	int		Width;
			public	int		Height;
			public	int		OffsetX;
			public	int		BearingX;
			public	int		AdvanveX;

			public  GlyphGeometry (int width, int height, int offsetX, int bearingX, int advanveX)
			{
				Width = width;
				Height = height;
				OffsetX = offsetX;
				BearingX = bearingX;
				AdvanveX = advanveX;
			}
		}

		// The number of precompiled character.
		private static int CHARACTER_RANGE = 256;
		
		// ###############################################################################
		// ### A T T R I B U T E S
		// ###############################################################################

		#region Attributes

		/// <summary>The disposed flag.</summary>
		private bool					_disposed;

		/// <summary>The font size in px.</summary>
    	private int						_fontSizeInPx;

		/// <summary>The vertical distance from the horizontal baseline to the highest ‘character’ coordinate in a font face.</summary>
    	private int						_ascendInPx;

		/// <summary>The flag, indicating whether the font supports horizontal text layout.</summary>
    	private bool					_supportHorzText;

    	/// <summary>The flag, indicating a monospace font.</summary>
    	private bool					_monospace;

		/// <summary>The texture, containing -all-glyphs- of the font.</summary>
		private int						_glyphsTextureId;

		/// <summary>The array of geometries, containing -all-glyphs- of the font.</summary>
		private GlyphGeometry[]			_glyphsGeometries;

		#endregion Attributes

		// ###############################################################################
		// ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
		// ###############################################################################

		#region Construction

		/// <summary>Initialize a new instance of the <see cref="FreeType.Font3D"/> class with font and size.</summary>
		/// <param name="fontFilePath">The font file path.</param>
		/// <param name="fontSizeInPx">The requested font size.</param>
	    public FtFont (string fontFilePath, int fontSizeInPx) 
	    {
	    	// Check whether compilation target fits to the operation system.
			if (sizeof (FreeTypeWrap.FT_Long) != System.IntPtr.Size)
			{
				if (System.IntPtr.Size == 4)
				{
					Console.WriteLine ("Wrong compilation options! Do not use 'X11_64' on 32 bit OS! Or wrong assembly attached. Use 32 bit assembly!");
					throw new DataMisalignedException ("Wrong compilation options! Do not use 'X11_64' on 32 bit OS! Or wrong assembly attached. Use 32 bit assembly!");
				}
				else
				{
					Console.WriteLine ("Wrong compilation options! Use 'X11_64' on 64 bit OS! Or wrong assembly attached. Use 64 bit assembly!");
					throw new DataMisalignedException ("Wrong compilation options! Use 'X11_64' on 64 bit OS! Or wrong assembly attached. Use 64 bit assembly!");
				}
			}

			// Check prerequisits.
			if (string.IsNullOrEmpty (fontFilePath) || string.IsNullOrEmpty(fontFilePath.Trim()))
			{
				Console.WriteLine ("The 'fontFilePath' must not be an empty string!");
				throw new ArgumentOutOfRangeException ("The 'fontFilePath' must not be an empty string!");
			}
			if (fontSizeInPx < 4 || fontSizeInPx > 72)
			{
				Console.WriteLine ("The 'fontSizeInPx' must be >= 4 and <= 72!");
				throw new ArgumentOutOfRangeException ("The 'fontSizeInPx' must be >= 4 and <= 72!");
			}

			// Save the size.
			_fontSizeInPx = fontSizeInPx;	    	

			// Create the library pointer.
			System.IntPtr libptr;			
			int ret = FT_Library.FT_Init_FreeType (out libptr);
			if (ret != 0)
			{
				Console.WriteLine ("FreeType library '" + FT.LIB + "' can not be initialized!");
				throw new TypeLoadException (typeof(FT_Library).Name);
			}

			// Create and load the font face.
			FT_Face face;
			System.IntPtr faceptr;		
			int retb = FT_Face.FT_New_Face (libptr, fontFilePath, 0, out faceptr);
			if (retb != 0)
			{
				// Dispose native pointers.
				FT_Library.FT_Done_FreeType(libptr);

				Console.WriteLine ("Font '" + fontFilePath + "' can not be initialized!");
				throw new TypeLoadException (typeof(FT_Face).Name);
			}

			// Get the font face.
			face = (FT_Face)Marshal.PtrToStructure (faceptr, typeof(FT_Face));
			if ((((int)face.face_flags) & ((int)((FT_Long)FT_FACE_FLAGS.FT_FACE_FLAG_SCALABLE))) !=
				(int)((FT_Long)FT_FACE_FLAGS.FT_FACE_FLAG_SCALABLE))
			{
				// Dispose native pointers.
				FT_Face.FT_Done_Face(faceptr);
				FT_Library.FT_Done_FreeType(libptr);

				Console.WriteLine ("Font '" + fontFilePath + "' must be scalable!");
				throw new ArgumentException ("Font '" + fontFilePath + "' is not scalable!");
			}
			if ((((int)face.face_flags) & ((int)((FT_Long)FT_FACE_FLAGS.FT_FACE_FLAG_HORIZONTAL))) !=
			    (int)((FT_Long)FT_FACE_FLAGS.FT_FACE_FLAG_HORIZONTAL))
				Console.WriteLine ("WARNING! The font '" + fontFilePath + "' is not suitable for horizontal text layout.");
			else
				_supportHorzText = true;

			// Extract some font metrics.
			if ((((int)face.face_flags) & ((int)((FT_Long)FT_FACE_FLAGS.FT_FACE_FLAG_FIXED_WIDTH))) !=
				(int)((FT_Long)FT_FACE_FLAGS.FT_FACE_FLAG_FIXED_WIDTH))
				_monospace = false;
			else
				_monospace = true;
			_ascendInPx  = (int)(((float)face.ascender) / (int)face.height * _fontSizeInPx + 0.49F);	// The face.ascende is measured in 26.6 format (* 64) before pixel size has been applied.

			// Freetype measures the font size in 1/64th of pixels for accuracy - charactes must be requested in size*64.
			FT_Face.FT_Set_Char_Size (faceptr, fontSizeInPx << 6, fontSizeInPx << 6, 96, 96);
			
			// Provide a reasonably accurate estimate for expected pixel sizes - this influences the glyph bitmap quality.
			FT_Face.FT_Set_Pixel_Sizes (faceptr, fontSizeInPx, fontSizeInPx);

			// Prepare the -all-glyphs- bitmap and glyph geometries.
			System.Drawing.Bitmap	glyphsBitmap = null;
			// Because drawing scaled glyphs can produce artefacts above/below the glyph, the bitmap has two extra scan-lines.
			glyphsBitmap = new System.Drawing.Bitmap (fontSizeInPx * CHARACTER_RANGE,
													   1 + fontSizeInPx + 1, // One extra scan-line above the glyph and one extra scan-line below the glyph.
													   System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Drawing.Imaging.BitmapData glyphsBitmapData = null;
			glyphsBitmapData = glyphsBitmap.LockBits(new System.Drawing.Rectangle(0, 0, glyphsBitmap.Width, glyphsBitmap.Height),
													  System.Drawing.Imaging.ImageLockMode.ReadWrite, glyphsBitmap.PixelFormat);

			_glyphsGeometries = new GlyphGeometry[CHARACTER_RANGE];

			// For each glyph: Transfer glyph image into -all-glyphs- bitmap and remember the glyph geometry.
			for (char c = '\0'; c < CHARACTER_RANGE; c++)
			{
				CompileCharacter (face, faceptr, glyphsBitmapData, c);
			}

			// Transfer -all-glyphs- bitmap to texture.
			GL.Enable(EnableCap.Texture2D);
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			_glyphsTextureId = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, _glyphsTextureId);
			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexImage2D  (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, glyphsBitmap.Width, glyphsBitmap.Height, 0,
				PixelFormat.Bgra, PixelType.UnsignedByte, glyphsBitmapData.Scan0);
			GL.Disable(EnableCap.Texture2D);

			// Clean up -all-glyphs- bitmap.
			glyphsBitmap.UnlockBits (glyphsBitmapData);
			if (glyphsBitmap != null)
				glyphsBitmap.Dispose ();

			// Dispose native pointers.
			FT_Face.FT_Done_Face(faceptr);
	    	FT_Library.FT_Done_FreeType(libptr);
	    }
		
		#endregion Construction	

		// ###############################################################################
		// ### D E S T R U C T I O N
		// ###############################################################################

		#region Destruction
		
		/// <summary>Release all resources used by the PaintEventArgs.</summary>
		/// <remarks>IDisposable implementation.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Internal (inheritable) dispose by parent.</summary>
        /// <param name="disposing">Determine whether Dispose() has been called by the user (true) or the runtime from inside the finalizer (false).</param>
        /// <remarks>If disposing equals false, no references to other objects shold be called.</remarks>
		protected virtual void Dispose (bool disposing)
		{
			if(_disposed)
				return;

			if (disposing)
			{
				GL.DeleteTextures (1, new int[] {_glyphsTextureId});
				_glyphsGeometries = null;
			}

			_disposed = true;
		}

		#endregion Destruction

		// ###############################################################################
		// ### P R O P E R T I E S
		// ###############################################################################

		#region Properties

		/// <summary>Get the font size in px.</summary>
		/// <value>The size.</value>
		public int Size
		{	get	{	return	_fontSizeInPx;	}	}

		/// <summary>Get whether the font supports horizontal text layout.</summary>
		public bool	SupportHorzText
		{	get	{	return	_supportHorzText;	}	}

		/// <summary>TGet whether the font is monospace.</summary>
		public bool Monospace
		{	get	{	return	_monospace;	}	}

		#endregion Properties

		// ###############################################################################
		// ### M E T H O D S
		// ###############################################################################

		#region Methods
	    
	    /// <summary>Compile a single character to a list of GL commands, that draw the character's glyph bitmap.</summary>
	    /// <param name="face">The font face, associated with the character to compile.</param>
		/// <param name="faceptr">The font face pointer, associated with the character to compile.</param>
		/// <param name="glyphsBitmapData">The bit,map data containing the glyph bitmap.</param>
		/// <param name="c">The character to compile to a list of GL commands, that draw the character's glyph bitmap.</param>
		/// <remarks>For details see: http://www.freetype.org/freetype2/docs/tutorial/step2.html</remarks>
		private void CompileCharacter (FT_Face face, System.IntPtr faceptr, System.Drawing.Imaging.BitmapData glyphsBitmapData, char c)
	    {
			int result;

	    	// Convert the number index to a character index.
	    	int index = FT_Face.FT_Get_Char_Index (faceptr, Convert.ToChar(c));
			if (index == 0)
			{
				Console.WriteLine ("Glyph index  for character '" + c + "' can not be determined!");
				return;
	    	}

			// Load a single glyph (indicated by the index of the glyph in the font file) into the glyph slot of a face object.
			// The FT_LOAD_TYPES.FT_LOAD_DEFAULT controls:
			// - PRINARILY: Look for a bitmap of the glyph, corresponding to the face's current size (if the glyph of the current size
			//              has already been loaded and provided as bitmap), provide bitmap data to the glyph slot and return integer 0.
			// - ALTERNATIVELY: Look for a scalable outline, load it from the font file, scale it to device pixels, ‘hint’ it to the
			//                  pixel grid (in order to optimize it), provide outline data to the glyph slot and return integer 0.
			// - FALLBACK: Return integer != 0.
			result = FT_Face.FT_Load_Glyph (faceptr, index, /*get metrics in 1/64th of pixels*/ FT_LOAD_TYPES.FT_LOAD_DEFAULT);
	    	if (result != 0)
			{
				Console.WriteLine ("Glyph with index '" + index + "' for character '" + c + "' can not be loaded!");
				return;
	    	}

			// Extract a glyph from the glyph slot. The 'face' is a managed mapping of the 'faceptr' and therefore it shares data.
			// The face.glyphrec is the raw unmanaged memory pointer.
			// The FT_Load_Glyph() has already provided the glyph to 'faceptr.glyphrec', that can also be accessed via 'face.glyphrec'.
			// The created glyph object must be released with FT_Done_Glyph().
	    	System.IntPtr glyphPointer;
			result = Glyph.FT_Get_Glyph (face.glyphrec, out glyphPointer);
			if (result != 0)
			{
				Console.WriteLine ("Glyph with index '" + index + "' for character '" + c + "' can not be accessed!");
				return;
	    	}

			// Convert a given glyph object to a bitmap glyph object.
			// The bitmap glyph object must be released with FT_Done_Glyph().
			result = Glyph.FT_Glyph_To_Bitmap (out glyphPointer,
											   /*8-bit anti-aliased pixmap*/ FT_RENDER_MODES.FT_RENDER_MODE_NORMAL,
											   /*origin*/ new FT_Vector (0, 0),
											   /*destroy non-bitmap glyph before replacing*/ (FT_Bool)1);
			if (result != 0)
			{
				Console.WriteLine ("Glyph with index '" + index + "' for character '" + c + "' can not be converted to bitmap!");
				return;
	    	}

			// Map glyph bitmap structure into managed code.
	    	FT_BitmapGlyph bitmapglyphStructure = (FT_BitmapGlyph)Marshal.PtrToStructure (glyphPointer, typeof(FT_BitmapGlyph));
			
			int glyphWidth = (int)(c == 32 ? _fontSizeInPx >> 1 : (int)(((long)bitmapglyphStructure.root.advance.x) / 65536));
			int ascentOffset = Math.Max (0, _ascendInPx - (int)bitmapglyphStructure.top);
			_glyphsGeometries[(int)c] = new GlyphGeometry ((int)bitmapglyphStructure.bitmap.width,
				(int)bitmapglyphStructure.bitmap.rows,
				c * _fontSizeInPx, (int)bitmapglyphStructure.left, glyphWidth);

			uint size = (bitmapglyphStructure.bitmap.width * bitmapglyphStructure.bitmap.rows);
			if (size <= 0)
				return;
			
			// Incorporate glyph bitmap bytes into managed code.
			byte[] currentGlyphBitmapBytes = new byte[size];
			Marshal.Copy (bitmapglyphStructure.bitmap.buffer, currentGlyphBitmapBytes, 0, currentGlyphBitmapBytes.Length);

			for (int countY = 0; countY < _fontSizeInPx + 2; countY++) // One extra scan-line above the glyph and one extra scan-line below the glyph.
			{
				for (int countX = 0; countX < _fontSizeInPx; countX++)
				{
					byte pixelComponent = (byte)0;
					if (countX < bitmapglyphStructure.bitmap.width &&
						countY - 1 - ascentOffset < bitmapglyphStructure.bitmap.rows  &&
						countY - 1 - ascentOffset >= 0)
						pixelComponent = currentGlyphBitmapBytes[countX + bitmapglyphStructure.bitmap.width * (countY - 1 - ascentOffset)];
					
					unsafe
					{
						byte* addr = (byte*)(glyphsBitmapData.Scan0) + countY * glyphsBitmapData.Stride + c * _fontSizeInPx * 4 + countX * 4;
						*(addr)     = pixelComponent;
						*(addr + 1) = pixelComponent;
						*(addr + 2) = pixelComponent;
						*(addr + 3) = pixelComponent;
					}
		    	}
	    	}

			// Clean up the bitmap glyph memory.
			Glyph.FT_Done_Glyph (glyphPointer);
	    } 

		/// <summary>Push the projection matrix to the matrix stack and set parallel projection according to the current viewport
		/// dimension, using an orthographic matrix. Prepare a GDI-text-mode alike coordinate system.</summary>
		/// <returns>The viewport dimension.<see cref="System.Drawing.Size"/> </returns>
		internal System.Drawing.Size PushProjectionMatrixAndSetParallelProjectionForViewport() 
		{
			int[] viewport = new int[4];

			// Save the coefficients of the six user-definable clipping planes.
			GL.PushAttrib(AttribMask.TransformBit);

			// Determine the viewport coordinates.
			GL.GetInteger(GetPName.Viewport, viewport);

			// Save the projection matrix.
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();

			// Reset/initialize the projection matrix.
			GL.LoadIdentity();

			// Set the GDI-text-mode alike coordinate system based on the viewport coordinates.
			GL.Ortho(viewport[0], viewport[2], viewport[3], viewport[1], -1, 1);

			// Recall the coefficients of the six user-definable clipping planes.
			GL.PopAttrib();

			return new System.Drawing.Size (viewport[2] - viewport[0], viewport[3] - viewport[1]);
		}

		/// <summary>Pop the projection matrix from the matrix stack.</summary>
		internal void PopProjectionMatrix ()
		{
			// Save the coefficients of the six user-definable clipping planes.
			GL.PushAttrib(AttribMask.TransformBit);

			// Recall the projection matrix.
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();

			// Recall the coefficients of the six user-definable clipping planes.
			GL.PopAttrib();			
		}

		/// <summary>Print the indicated text (left to right) without any formatting at the indicated x and y coordinate.</summary>
		/// <param name="text">The text to print.</param>
		/// <param name="x">The x coordinate of the first character, given in a GDI-text-mode alike coordinate system.</param>
		/// <param name="y">The y coordinate of the bottom line, given in a GDI-text-mode alike coordinate system.</param>
		/// <returns>The extents of the drawn string.</returns>
		/// <remarks>Assume a GDI-text-mode alike coordinate system for x and y coordinate (origin at left top corner) with positive
		/// x-axis from left to right and positive y-axis from top to bottom.</remarks>
		public System.Drawing.Size DrawString (string text, int x, int y)
		{
			System.Drawing.Size extends = new System.Drawing.Size (0, 0);

			try
			{
				int startX = x;

				// Prepare projection matrix.
				PushProjectionMatrixAndSetParallelProjectionForViewport();

				// Prepare modelview matrix.
				GL.MatrixMode(MatrixMode.Modelview);
				GL.PushMatrix();
				GL.LoadIdentity();

				// Set texture handling.
				GL.Enable (EnableCap.Blend);
				GL.BlendFunc (BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				GL.Enable (EnableCap.Texture2D);
				GL.BindTexture (TextureTarget.Texture2D, _glyphsTextureId);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				GL.Begin(PrimitiveType.Quads);
				for (int charIndex = 0; charIndex < text.Length; charIndex++)
				{
					// Realize line-break.
					if ((byte)text[charIndex] == (byte)'\n')
					{
						// Safe horz. extent.
						extends.Width = Math.Max (extends.Width, x);
						// Add vert. extent.
						extends.Height += _fontSizeInPx;

						// Reposition pen.
						x = startX;
						y += _fontSizeInPx;

						// Skip glyph drawing.
						continue;
					}

					int glyph = (int)text[charIndex];

					// Identify character conversion problems, caused by the CHARACTER_RANGE!
					if (glyph != (int)((byte)text[charIndex]))
						glyph = (int)((byte)text[charIndex]);

					GlyphGeometry gg = _glyphsGeometries[glyph];

					// Extract the requested glyph texture from the -all-glyphs- texture.
					double glyphX1 = ((double)glyph) / CHARACTER_RANGE;
					double glyphY1 = 0.0;
					double glyphX2 = glyphX1 + 1.0 / CHARACTER_RANGE;
					double glyphY2 = 1.0;

					// Place the requested glyph texture into the viewport.
					int    viewX1 = x + gg.BearingX;
					int    viewY1 = y;
					int    viewX2 = viewX1 + _fontSizeInPx;
					int    viewY2 = viewY1 + _fontSizeInPx;

					GL.TexCoord2(glyphX1, glyphY2);		GL.Vertex2(viewX1, viewY2);
					GL.TexCoord2(glyphX2, glyphY2);		GL.Vertex2(viewX2, viewY2);
					GL.TexCoord2(glyphX2, glyphY1);		GL.Vertex2(viewX2, viewY1);
					GL.TexCoord2(glyphX1, glyphY1);		GL.Vertex2(viewX1, viewY1);

					x += gg.AdvanveX - gg.BearingX;
				}
				GL.End();

				// Reset texture handling.
				//GL.BindTexture (TextureTarget.Texture2D, 0);
				GL.Disable (EnableCap.Texture2D);
				GL.Disable (EnableCap.Blend);

				// Restore modelview matrix.
				GL.PopMatrix();

				// Restore projection matrix.
				PopProjectionMatrix();
			}
			catch (Exception e)
			{
				Console.WriteLine (e.Message);
				Console.WriteLine (e.StackTrace);
			}
			return extends;
		}

		#endregion Methods
				
	}
	
}