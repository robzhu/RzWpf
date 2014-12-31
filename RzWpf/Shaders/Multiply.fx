sampler2D ourImage : register(s0);

// the color to multiply across the entire image.
float4 Color : register(C0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  float4 color;
  color = tex2D( ourImage , locationInSource.xy);

  color.r = color.r * Color.r;
  color.g = color.g * Color.g;
  color.b = color.b * Color.b;
  
  return color;
}