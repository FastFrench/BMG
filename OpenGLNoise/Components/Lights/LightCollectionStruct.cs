using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenGLNoise.Lights;

namespace OpenGLNoise.Components.Lights
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = sizeof(float), Size = sizeof(float) * 4)]
  public struct GlobalSubStruct
  {
    public float Gamma;
    public int NbLights;
  };

  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = sizeof(float) * 4)]
  public struct LightCollectionStruct
  {
    public GlobalSubStruct GlobalData;
    public const int MAXLIGHTCOUNT = 3;
    public LightStruct[] Lights;
    public void Init(LightDataCollection col, float gamma = 2.2f)
    {
      Lights = new LightStruct[MAXLIGHTCOUNT];
      for(int i=0; i<col.Count; i++)
        col[i].FillLightStructStruct(ref Lights[i]);
      GlobalData.Gamma = gamma;
      GlobalData.NbLights = col.Count;
    }

    public int Size(bool max = false)
    {
      return Marshal.SizeOf<LightStruct>() * (max ? MAXLIGHTCOUNT : GlobalData.NbLights) + Marshal.SizeOf<GlobalSubStruct>();
    }
  }
}
