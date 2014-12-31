sampler2D ourImage : register(s0);

// ranges from 0 to 1, where 0 does not affect the image, and 1 makes the image completely black.
float Darkness : register(C0);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  float4 color;
  color = tex2D( ourImage , locationInSource.xy);
  
  float factor = ( 1 - Darkness );
  if( factor < 0 ) factor = 0;
  
  color.r = color.r * factor;
  color.g = color.g * factor;
  color.b = color.b * factor;
  
  return color;
}
    