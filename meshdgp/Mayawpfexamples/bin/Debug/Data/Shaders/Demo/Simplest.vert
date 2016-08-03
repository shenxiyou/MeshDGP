  
   uniform int type;
   void main(void)  
   {
	   if(type==1)
	   {
		 gl_Position =ftransform();
	   }
	   else if(type==2)
	   {
	     gl_Position =gl_ModelViewProjectionMatrix * gl_Vertex;  
	   }
	   else if(type==3)
	  {
	      gl_Position =gl_ProjectionMatrix * gl_ModelViewMatrix * gl_Vertex; 
	  }
   }