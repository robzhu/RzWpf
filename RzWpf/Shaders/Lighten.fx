sampler2D image : register(s0);

// the color to multiply across the entire image.
float4 Color : register(C0);
float Intensity : register(C1);

float4 main(float2 locationInSource : TEXCOORD) : COLOR
{
  float4 color;
  color = tex2D( image , locationInSource.xy);

  if( color.a != 0 )
    {
    	color.r = max( color.r , Color.r * Intensity );
  		color.g = max( color.g , Color.g * Intensity );
  		color.b = max( color.b , Color.b * Intensity );
    }
  
  return color;
}