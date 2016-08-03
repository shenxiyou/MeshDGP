using System;
 

namespace GraphicResearchHuiZhao 
{
	public class ArcBall
	{
		public enum MotionType { None, Rotation, Pan, Scale }

		private MotionType type = MotionType.None;
		private Vector2D startPt, endPt;
		private Vector3D startVec;
		private Vector3D endVec;
		private Vector4D quat;
		private double w, h;
		private double adjustWidth;
		private double adjustHeight;

		public ArcBall(double w, double h)
		{
			SetBounds(w,h);
		}

		public void SetBounds(double w, double h)
		{
			double b = (w<h)?w:h;
			this.w = w / 2.0;
			this.h = h / 2.0;
			this.adjustWidth = 1.0 / ((b - 1.0) * 0.5);
			this.adjustHeight = 1.0 / ((b - 1.0) * 0.5);
		}

		public void Click(Vector2D pt, MotionType type)
		{
			this.startPt = pt;
			this.startVec = MapToSphere(pt);
			this.type = type;
		}

		public void Drag(Vector2D pt)
		{
			endPt = pt;
			endVec = MapToSphere(pt);

			double epsilon = 1.0e-5;
			Vector3D prep = startVec.Cross(endVec);

			if (prep.Length() > epsilon)
				quat = new Vector4D(prep, startVec.Dot(endVec));
			else
				quat = new Vector4D();
		}


		public void End()
		{
			quat = new Vector4D();
			type = MotionType.None;
		}

		public Matrix4D CreateMatrix()
		{
            if (type == MotionType.Rotation)
            {
                Quaternion q = new Quaternion(quat);
                return Matrix4D.RotationQuaternion(q);
            }			 

			if (type == MotionType.Scale)
			{
				Matrix4D m = Matrix4D.IdentityMatrix;
				m[0,0] = m[1,1] = m[2,2] = 
                    1.0 + (endPt.x - startPt.x) * adjustWidth;
				return m;
			}
			if (type == MotionType.Pan)
			{
				Matrix4D m = Matrix4D.IdentityMatrix;
				m[3,0] = endPt.x - startPt.x;
				m[3,1] = endPt.y - startPt.y;
				return m;
			}
			return Matrix4D.IdentityMatrix;
		}




		private Vector3D MapToSphere(Vector2D pt)
		{
			Vector2D v = new Vector2D();
			v.x = (w - pt.x) * adjustWidth;
			v.y = (h - pt.y) * adjustHeight;

			double length = v.Dot(v);
			if (length > 1.0)
			{
				double norm = 1.0 / Math.Sqrt(length);
				return new Vector3D(v.x * norm, v.y * norm, 0);
			}
			else
			{
				return new Vector3D(v.x, v.y, Math.Sqrt(1.0 - length));
			}
		}


      
        //private Matrix4D QuatToMatrix4D(Vector4D q)
        //{
        //    double n = q.Dot(q);
        //    double s = (n > 0.0) ? (2.0 / n) : 0.0f;

        //    double xs, ys, zs;
        //    double wx, wy, wz;
        //    double xx, xy, xz;
        //    double yy, yz, zz;
        //    xs = q.x * s; ys = q.y * s; zs = q.z * s;
        //    wx = q.w * xs; wy = q.w * ys; wz = q.w * zs;
        //    xx = q.x * xs; xy = q.x * ys; xz = q.x * zs;
        //    yy = q.y * ys; yz = q.y * zs; zz = q.z * zs;

        //    Matrix4D m = new Matrix4D();
        //    m[0, 0] = 1.0 - (yy + zz); m[1, 0] = xy - wz; m[2, 0] = xz + wy;
        //    m[0, 1] = xy + wz; m[1, 1] = 1.0 - (xx + zz); m[2, 1] = yz - wx;
        //    m[0, 2] = xz - wy; m[1, 2] = yz + wx; m[2, 2] = 1.0 - (xx + yy);
        //    m[3, 3] = 1.0;
        //    return m;
        //}


        //public double GetScale()
        //{
        //    if (type == MotionType.Scale)
        //        return 1.0 + (edPt.x - stPt.x) * adjustWidth;
        //    else
        //        return 1.0;
        //}

        //private Matrix3D QuatToMatrix3d(Vector4D q)
        //{
        //    double n = q.Dot(q);
        //    double s = (n > 0.0) ? (2.0 / n) : 0.0f;

        //    double xs, ys, zs;
        //    double wx, wy, wz;
        //    double xx, xy, xz;
        //    double yy, yz, zz;
        //    xs = q.x * s;  ys = q.y * s;  zs = q.z * s;
        //    wx = q.w * xs; wy = q.w * ys; wz = q.w * zs;
        //    xx = q.x * xs; xy = q.x * ys; xz = q.x * zs;
        //    yy = q.y * ys; yz = q.y * zs; zz = q.z * zs;

        //    Matrix3D m = new Matrix3D();
        //    m[0,0] = 1.0 - (yy + zz); m[1,0] =        xy - wz;  m[2,0] =        xz + wy;
        //    m[0,1] =        xy + wz;  m[1,1] = 1.0 - (xx + zz); m[2,1] =        yz - wx;
        //    m[0,2] =        xz - wy;  m[1,2] =        yz + wx;  m[2,2] = 1.0 - (xx + yy);
        //    return m;
        //}
	}
}
