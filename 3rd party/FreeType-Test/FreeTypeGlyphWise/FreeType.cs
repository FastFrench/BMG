using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System.Runtime.InteropServices;

namespace FreeTypeWrap
{

#if (X11_64)
	public enum FT_String		: sbyte		{	};  // X11 64/32 Bit: 1 Byte:                  -127 to 127
	public enum FT_Char			: sbyte		{	};  // X11 64/32 Bit: 1 Byte:                  -127 to 127
	public enum FT_Uchar		: byte		{	};  // X11 64/32 Bit: 1 Byte:                     0 to 255
	public enum FT_Bool			: byte		{	};  // X11 64/32 Bit: 1 Byte:                     0 to 255

	public enum FT_Short		: short		{	};  // X11 64/32 Bit: 2 Byte:                -32767 to 32767
	public enum FT_UShort		: ushort	{	};  // X11 64/32 Bit: 2 Byte:                     0 to 65535
	public enum FT_Int			: int		{	};  // X11 64/32 Bit: 4 Bytes:       -2.147.483.648 to 2.147.483.647
	public enum FT_Uint			: uint		{	};  // X11 64/32 Bit: 4 Bytes:                    0 to 4294967295

	public enum FT_Long			: long		{	};  // X11 64    Bit: 8 Bytes: -9223372036854775807 to 9223372036854775807
	public enum FT_Ulong		: ulong		{	};  // X11 64    Bit: 8 Bytes:                    0 to 18446744073709551615

	public enum FT_Pos			: long		{	};  // X11 64    Bit: 8 Bytes: -9223372036854775807 to 9223372036854775807
#else
	public enum FT_String		: sbyte		{	};  // X11 64/32 Bit: 1 Byte:                  -127 to 127
	public enum FT_Char			: sbyte		{	};  // X11 64/32 Bit: 1 Byte:                  -127 to 127
	public enum FT_Uchar		: byte		{	};  // X11 64/32 Bit: 1 Byte:                     0 to 255
	public enum FT_Bool			: byte		{	};  // X11 64/32 Bit: 1 Byte:                     0 to 255

	public enum FT_Short		: short		{	};  // X11 64/32 Bit: 2 Byte:                -32767 to 32767
	public enum FT_UShort		: ushort	{	};  // X11 64/32 Bit: 2 Byte:                     0 to 65535
	public enum FT_Int			: int		{	};  // X11 64/32 Bit: 4 Bytes:       -2.147.483.648 to 2.147.483.647
	public enum FT_Uint			: uint		{	};  // X11 64/32 Bit: 4 Bytes:                    0 to 4294967295

	public enum FT_Long			: int		{	};  // X11    32 Bit: 4 Bytes:       -2.147.483.648 to 2.147.483.647
	public enum FT_Ulong		: uint		{	};  // X11    32 Bit: 4 Bytes:                    0 to 4294967295

	public enum FT_Pos			: int		{	};  // X11    32 Bit: 4 Bytes:       -2.147.483.648 to 2.147.483.647
#endif

	public class FT
	{
		public const string LIB = "libfreetype.so"; // freetype.dll

		public static FT_Int FT_IMAGE_TAG (char b3, char b2, char b1, char b0)
		{	return (FT_Int)(((byte)b3) * 16777216 + ((byte)b2) * 65536 + ((byte)b1) * 256 + ((byte)b0));
		}
		
		public static FT_Int FT_GLYPH_FORMAT_NONE      = FT_IMAGE_TAG ((char)0, (char)0, (char)0, (char)0);
		public static FT_Int FT_GLYPH_FORMAT_COMPOSITE = FT_IMAGE_TAG ('c', 'o', 'm', 'p');
		public static FT_Int FT_GLYPH_FORMAT_BITMAP    = FT_IMAGE_TAG ('b', 'i', 't', 's');
		public static FT_Int FT_GLYPH_FORMAT_OUTLINE   = FT_IMAGE_TAG ('o', 'u', 't', 'l');
		public static FT_Int FT_GLYPH_FORMAT_PLOTTER   = FT_IMAGE_TAG ('p', 'l', 'o', 't');
	}
	
	public enum FT_LOAD_TYPES
	{
		FT_LOAD_DEFAULT = 0,
		FT_LOAD_NO_SCALE = 1
	}

	public enum FT_RENDER_MODES
	{
		FT_RENDER_MODE_NORMAL = 0,
		FT_RENDER_MODE_LIGHT = 1				
	}

	public enum FT_Glyph_Format : int // FT_Int
	{
		FT_GLYPH_FORMAT_NONE      = 0,				// must be equal to FT.FT_GLYPH_FORMAT_NONE.
		FT_GLYPH_FORMAT_COMPOSITE = 1668246896,		// must be equal to FT.FT_GLYPH_FORMAT_COMPOSITE.
		FT_GLYPH_FORMAT_BITMAP    = 1651078259,		// must be equal to FT.FT_GLYPH_FORMAT_BITMAP.
		FT_GLYPH_FORMAT_OUTLINE   = 1869968492,		// must be equal to FT.FT_GLYPH_FORMAT_OUTLINE.
		FT_GLYPH_FORMAT_PLOTTER   = 1886154612		// must be equal to FT.FT_GLYPH_FORMAT_PLOTTER.
	}

	public enum FT_FACE_FLAGS
	{
		FT_FACE_FLAG_SCALABLE          = ( 1 <<  0 ),	// Indicates that the face contains outline glyphs.
		FT_FACE_FLAG_FIXED_SIZES       = ( 1 <<  1 ),	// Indicates that the face contains bitmap strikes.
		FT_FACE_FLAG_FIXED_WIDTH       = ( 1 <<  2 ),	// Indicates that the face contains fixed-width characters (monospace).
		FT_FACE_FLAG_SFNT              = ( 1 <<  3 ),	// Indicates that the face uses the ‘sfnt’ storage scheme.
		FT_FACE_FLAG_HORIZONTAL        = ( 1 <<  4 ),	// Indicates that the face contains horizontal glyph metrics.
		FT_FACE_FLAG_VERTICAL          = ( 1 <<  5 ),	// Indicates that the face contains vertical glyph metrics.
		FT_FACE_FLAG_KERNING           = ( 1 <<  6 ),	// Indicates that the face contains kerning information.
		FT_FACE_FLAG_FAST_GLYPHS       = ( 1 <<  7 ),	// DEPRECATED.
		FT_FACE_FLAG_MULTIPLE_MASTERS  = ( 1 <<  8 ),
		FT_FACE_FLAG_GLYPH_NAMES       = ( 1 <<  9 ),
		FT_FACE_FLAG_EXTERNAL_STREAM   = ( 1 << 10 ),
		FT_FACE_FLAG_HINTER            = ( 1 << 11 ),
		FT_FACE_FLAG_CID_KEYED         = ( 1 << 12 ),
		FT_FACE_FLAG_TRICKY            = ( 1 << 13 ),
		FT_FACE_FLAG_COLOR             = ( 1 << 14 ),
	}

	public enum FT_STYLE_FLAGS
	{
		FT_STYLE_FLAG_ITALIC           = ( 1 << 0 ),
		FT_STYLE_FLAG_BOLD             = ( 1 << 1 )
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_Library
	{
		public IntPtr			memory;
		public FT_Generic		generic;
		public int				major;
		public int				minor;
		public int				patch;
		public uint				modules;
		public System.IntPtr	module0, module1, module2, module3, module4, module5, module6, module7, module8, module9, module10;
		public System.IntPtr	module11, module12, module13, module14, module15, module16, module17, module18, module19, module20;
		public System.IntPtr	module21, module22, module23, module24, module25, module26, module27, module28, module29, module30;
		public System.IntPtr	module31;
		public FT_ListRec		renderers;
		public System.IntPtr	renderer;
		public System.IntPtr	auto_hinter;
		public System.IntPtr	raster_pool;
		public long				raster_pool_size;
		public System.IntPtr	debug0, debug1, debug2, debug3;
			
		[DllImport (FT.LIB, EntryPoint="FT_Init_FreeType", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int FT_Init_FreeType (out System.IntPtr lib);

		[DllImport (FT.LIB, EntryPoint="FT_Done_FreeType", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern void FT_Done_FreeType (System.IntPtr lib);
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_Generic
	{
		public System.IntPtr	data;
		public System.IntPtr	finalizer;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_BBox
	{
		public FT_Pos	xMin, yMin;
		public FT_Pos	xMax, yMax;	
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_ListRec
	{
		public System.IntPtr head;
		public System.IntPtr tail;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_Vector
	{
		public FT_Pos	x;
		public FT_Pos	y;

		public FT_Vector (int vX, int vY)
		{
			x = (FT_Pos) vX;
			y = (FT_Pos) vY;
		}

		public FT_Vector (long vX, long vY)
		{
			x = (FT_Pos) vX;
			y = (FT_Pos) vY;
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_Face
	{
		public FT_Long									num_faces;
		public FT_Long									face_index;

		public FT_Long									face_flags;			/* See: FT_FACE_FLAGS */
		public FT_Long									style_flags;		/* See: FT_STYLE_FLAGS */

		public FT_Long									num_glyphs;

		[MarshalAs(UnmanagedType.LPStr)]public	string	family_name;		/* FT_String[] */
		[MarshalAs(UnmanagedType.LPStr)]public	string	style_name;			/* FT_String[] */

		public FT_Int									num_fixed_sizes;
		public IntPtr									available_sizes;	/* FT_Bitmap_Size* */

		public FT_Int									num_charmaps;
		public IntPtr 									charmaps;			/* FT_CharMap* */

		public FT_Generic								generic;			/* FT_Generic */

		/*# The following are only relevant to scalable outlines. */
		public FT_BBox									box;
		public FT_UShort								units_per_EM;
		public FT_Short									ascender;			// The vertical distance from the horizontal baseline to the highest ‘character’ coordinate in a font face, measured in 26.6 format (* 64) before pixel size has been applied.
		public FT_Short									descender;			// The vertical distance from the horizontal baseline to the lowest ‘character’ coordinate in a font face, measured in 26.6 format (* 64) before pixel size has been applied.
		public FT_Short									height;				// The default line spacing (i.e., the baseline-to-baseline distance) when writing text with this font, measured in 26.6 format (* 64) before pixel size has been applied.

		public FT_Short									max_advance_width;
		public FT_Short									max_advance_height;

		public FT_Short									underline_position;
		public FT_Short									underline_tickness;

		public /* FT_GlyphSlot */		System.IntPtr	glyphrec;
		public /* FT_Size */			System.IntPtr 	size;
		public /* FT_CharMap */			System.IntPtr   charmap;

		/*@private begin */
		public /* FT_Driver */			System.IntPtr	driver;
		public /* FT_Memory */			System.IntPtr	memory;
		public /* FT_Stream */			System.IntPtr	stream;

		public FT_ListRec				sizes_list;

		public FT_Generic				autohint;
		public /* void* */				System.IntPtr	extensions;

		public /* FT_Face_Internal */	System.IntPtr 	internal_face;

		/*@private end */

		[DllImport (FT.LIB, EntryPoint="FT_New_Face", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int FT_New_Face (System.IntPtr lib, string fname, int index, out System.IntPtr face);

		[DllImport (FT.LIB, EntryPoint="FT_Set_Char_Size", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern void FT_Set_Char_Size (System.IntPtr face, int width, int height, int horz_resolution, int vert_resolution);

		[DllImport (FT.LIB, EntryPoint="FT_Set_Pixel_Sizes", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern void FT_Set_Pixel_Sizes (System.IntPtr face, int pixel_width, int pixel_height);

		[DllImport (FT.LIB, EntryPoint="FT_Done_Face", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern void FT_Done_Face (System.IntPtr face);

		[DllImport (FT.LIB, EntryPoint="FT_Get_Char_Index", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int FT_Get_Char_Index (System.IntPtr face, char c); 

		[DllImport (FT.LIB, EntryPoint="FT_Load_Glyph", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int FT_Load_Glyph (System.IntPtr face, int index, FT_LOAD_TYPES flags);
	}
	
	[StructLayout (LayoutKind.Sequential)]
	public struct FT_GlyphRec
	{
		public System.IntPtr 			library;	
		public System.IntPtr  			clazz;		
		public FT_Int					format;		
		public FT_Vector				advance;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_GlyphClass
	{
		public	FT_Long			size;			
		public	FT_Glyph_Format format;			
		public	System.IntPtr	init;
		public	System.IntPtr	done;
		public	System.IntPtr 	copy;
		public	System.IntPtr	transform;
		public	System.IntPtr	bbox;
		public	System.IntPtr	prepare;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_BitmapGlyph
	{
		public FT_GlyphRec 		root;
		public FT_Int			left;		// The left-side bearing, i.e., the horizontal distance from the current pen position to the left border of the glyph bitmap.
		public FT_Int			top;		// The top-side bearing, i.e., the vertical distance from the current pen position to the top border of the glyph bitmap. This distance is positive for upwards y! (Distance from first pixel to base-line.)
		public FT_Bitmap 		bitmap;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct FT_Bitmap
	{
		public uint		rows;
		public uint		width;
		public int		pitch;
		public IntPtr	buffer;
		public ushort	num_grays;
		public byte		pixel_mode;
		public byte		palette_mode;
		public IntPtr	palette;	
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct Glyph
	{
		[DllImport (FT.LIB, EntryPoint="FT_Get_Glyph", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int FT_Get_Glyph (System.IntPtr glyphrec, out System.IntPtr glyph);

		[DllImport (FT.LIB, EntryPoint="FT_Glyph_To_Bitmap", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern int FT_Glyph_To_Bitmap (out System.IntPtr glyph, FT_RENDER_MODES render_mode, FT_Vector origin, FT_Bool destroy);
		
		[DllImport (FT.LIB, EntryPoint="FT_Done_Glyph", CallingConvention=CallingConvention.Cdecl), System.Security.SuppressUnmanagedCodeSecurity]
		public static extern void FT_Done_Glyph (System.IntPtr glyph);
	}

}