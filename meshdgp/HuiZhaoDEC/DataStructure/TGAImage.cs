using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GraphicResearchHuiZhao
{

    public class TGAImage
    {
        public TGAImage()
        {

        }

        public TGAImage(string p_FileFullName)
        {
            if (System.IO.File.Exists(p_FileFullName))
            {
                LoadData(File.ReadAllBytes(p_FileFullName));
            }
        }

        public double[] Pixels;

        #region 属性
        /// <summary>
        /// ID区域大小 默认为0
        /// </summary>
        private byte m_IDSize = 0;
        /// <summary>
        /// 色彩表类型 默认为0
        /// </summary>
        private byte m_ColorTableType = 0;
        /// <summary>
        /// 色彩表类型 默认为2
        /// </summary>
        private byte m_ImageType = 2;
        /// <summary>
        /// 色彩表类型开始位置
        /// </summary>
        private ushort m_ColorTableIndex = 0;
        /// <summary>
        /// 色彩表长度
        /// </summary>
        private ushort m_ColorTableCount = 0;
        /// <summary>
        /// 色彩表大小
        /// </summary>
        private byte m_ColorTableSize = 24;
        /// <summary>
        /// 色彩表类型开始位置
        /// </summary>
        private ushort m_ImageX = 0;
        /// <summary>
        /// 色彩表长度
        /// </summary>
        private ushort m_ImageY = 0;
        /// <summary>
        /// 色彩表类型开始位置
        /// </summary>
        private ushort m_ImageWidth = 0;
        /// <summary>
        /// 色彩表长度
        /// </summary>
        private ushort m_ImageHeight = 0;
        /// <summary>
        /// 每像素位数
        /// </summary>
        private byte m_PixSize = 0;
        /// <summary>
        /// 描述占位符
        /// </summary>
        private byte m_Remark = 0;
        /// <summary>
        /// 读的位置
        /// </summary>
        private uint m_ReadIndex = 0;
        /// <summary>
        /// 图形
        /// </summary>
        private Bitmap m_Image;
        #endregion

        /// <summary>
        /// 图形
        /// </summary>
        public Bitmap Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                m_Image = value;
                if (value != null)
                {
                    switch (value.PixelFormat)
                    {
                        case PixelFormat.Format8bppIndexed:
                            m_ColorTableType = 1;
                            m_ImageType = 1;
                            m_ColorTableCount = 256;
                            m_PixSize = 8;
                            m_Remark = 32;
                            break;
                        case PixelFormat.Format32bppArgb:
                            m_ColorTableType = 0;
                            m_ImageType = 2;
                            m_ColorTableCount = 0;
                            m_PixSize = 32;
                            m_Remark = 32;
                            break;
                        default:
                            m_ColorTableType = 0;
                            m_ImageType = 2;
                            m_ColorTableCount = 0;
                            m_PixSize = 24;
                            m_Remark = 32;
                            break;
                    }
                    m_ImageWidth = (ushort)value.Width;
                    m_ImageHeight = (ushort)value.Height;
                }
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="p_TGABytes"></param>
        private void LoadData(byte[] p_TGABytes)
        {
            m_IDSize = p_TGABytes[0];
            m_ColorTableType = p_TGABytes[1];
            m_ImageType = p_TGABytes[2];
            m_ColorTableIndex = BitConverter.ToUInt16(p_TGABytes, 3);
            m_ColorTableCount = BitConverter.ToUInt16(p_TGABytes, 5);
            m_ColorTableSize = p_TGABytes[7];
            m_ImageX = BitConverter.ToUInt16(p_TGABytes, 8);
            m_ImageY = BitConverter.ToUInt16(p_TGABytes, 10);
            m_ImageWidth = BitConverter.ToUInt16(p_TGABytes, 12);
            m_ImageHeight = BitConverter.ToUInt16(p_TGABytes, 14);
            m_PixSize = p_TGABytes[16];
            m_Remark = p_TGABytes[17];
            m_ReadIndex = 18;
            LoadImage(p_TGABytes);
        }

        private void LoadImage(byte[] p_TAGBytes)
        {
            switch (m_PixSize)
            {
                case 24:
                    m_Image = new Bitmap(m_ImageWidth, m_ImageHeight, PixelFormat.Format24bppRgb);
                    LoadImageNotRLE(p_TAGBytes, 3);
                    break;
                case 32:
                    m_Image = new Bitmap(m_ImageWidth, m_ImageHeight, PixelFormat.Format24bppRgb);
                    LoadImageNotRLE(p_TAGBytes, 4);
                    break;
                case 8:
                    m_Image = new Bitmap(m_ImageWidth, m_ImageHeight, PixelFormat.Format8bppIndexed);
                    Pixels = new double[m_ImageWidth * m_ImageHeight];

                    for (int i = 0; i < m_ImageHeight; i++)
                    {
                        for (int j = 0; j < m_ImageWidth; j++)
                        {
                            Pixels[j + m_ImageWidth * i] = (double)p_TAGBytes[m_ReadIndex + i * m_ImageWidth + j] / 255;
                        }
                    }

                    ColorPalette _Palette = m_Image.Palette;
                    for (int i = 0; i != m_ColorTableCount; i++)
                    {
                        _Palette.Entries[i] = Color.FromArgb(255, p_TAGBytes[m_ReadIndex + 2], p_TAGBytes[m_ReadIndex + 1], p_TAGBytes[m_ReadIndex]);
                        m_ReadIndex += 3;
                    }
                    m_Image.Palette = _Palette;
                    LoadImageNotRLE(p_TAGBytes, 1);
                    break;
            }
        }

        /// <summary>
        /// 获取非RLE压缩的图形
        /// </summary>
        /// <param name="p_TagBytes"></param>
        /// <param name="p_Size"></param>
        private void LoadImageNotRLE(byte[] p_TagBytes, int p_Size)
        {
            int _Index = 0;
            PixelFormat _Format = PixelFormat.Format24bppRgb;
            switch (p_Size)
            {
                case 3:
                    _Format = PixelFormat.Format24bppRgb;
                    break;
                case 4:
                    _Format = PixelFormat.Format32bppArgb;
                    break;
                case 1:
                    _Format = PixelFormat.Format8bppIndexed;
                    break;
            }
            BitmapData _ImageData = m_Image.LockBits(new Rectangle(0, 0, m_ImageWidth, m_ImageHeight), ImageLockMode.ReadWrite, _Format);
            byte[] _ValueBytes = new byte[_ImageData.Stride * _ImageData.Height];
            for (int i = 0; i != _ImageData.Height; i++)
            {
                _Index = _ImageData.Stride * i;

                Array.Copy(p_TagBytes, m_ReadIndex, _ValueBytes, _Index, _ImageData.Width * p_Size);
                m_ReadIndex += (uint)_ImageData.Width * (uint)p_Size;
            }
            Marshal.Copy(_ValueBytes, 0, _ImageData.Scan0, _ValueBytes.Length);
            m_Image.UnlockBits(_ImageData);
        }

        /// <summary>
        /// 保存图形为TGA
        /// </summary>
        /// <returns></returns>
        private byte[] SaveImageToTGA()
        {
            if (m_Image == null) return null;
            MemoryStream _ImageMemory = new MemoryStream();
            _ImageMemory.WriteByte(m_IDSize);
            _ImageMemory.WriteByte(m_ColorTableType);
            _ImageMemory.WriteByte(m_ImageType);
            _ImageMemory.Write(BitConverter.GetBytes(m_ColorTableIndex), 0, 2);
            _ImageMemory.Write(BitConverter.GetBytes(m_ColorTableCount), 0, 2);
            _ImageMemory.WriteByte(m_ColorTableSize);
            _ImageMemory.Write(BitConverter.GetBytes(m_ImageX), 0, 2);
            _ImageMemory.Write(BitConverter.GetBytes(m_ImageY), 0, 2);
            _ImageMemory.Write(BitConverter.GetBytes(m_ImageWidth), 0, 2);
            _ImageMemory.Write(BitConverter.GetBytes(m_ImageHeight), 0, 2);
            _ImageMemory.WriteByte(m_PixSize);
            _ImageMemory.WriteByte(m_Remark);
            int _ColorSize = 0;
            Bitmap _SaveBitmap = m_Image;
            switch (_SaveBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    _ColorSize = 3;
                    break;
                case PixelFormat.Format8bppIndexed:
                    _ColorSize = 1;
                    for (int i = 0; i != m_ColorTableCount; i++)
                    {
                        _ImageMemory.WriteByte(m_Image.Palette.Entries[i].B);
                        _ImageMemory.WriteByte(m_Image.Palette.Entries[i].G);
                        _ImageMemory.WriteByte(m_Image.Palette.Entries[i].R);
                    }
                    break;
                case PixelFormat.Format32bppArgb:
                    _ColorSize = 4;
                    break;
                default:
                    _SaveBitmap = new Bitmap(m_Image.Width, m_Image.Height, PixelFormat.Format24bppRgb);
                    Graphics _Graphics = Graphics.FromImage(_SaveBitmap);
                    _Graphics.DrawImage(m_Image, new Rectangle(0, 0, _SaveBitmap.Width, _SaveBitmap.Height));
                    _Graphics.Dispose();
                    _ColorSize = 3;
                    break;
            }
            BitmapData _ImageData = _SaveBitmap.LockBits(new Rectangle(0, 0, _SaveBitmap.Width, _SaveBitmap.Height), ImageLockMode.ReadWrite, _SaveBitmap.PixelFormat);
            byte[] _ValueBytes = new byte[_ImageData.Stride * _ImageData.Height];
            Marshal.Copy(_ImageData.Scan0, _ValueBytes, 0, _ValueBytes.Length);
            _SaveBitmap.UnlockBits(_ImageData);
            int _Index = 0;
            for (int i = 0; i != _ImageData.Height; i++)
            {
                _Index = _ImageData.Stride * i;
                _ImageMemory.Write(_ValueBytes, _Index, _ColorSize * _ImageData.Width);
            }
            return _ImageMemory.ToArray();
        }

        /// <summary>
        /// 保存图形到文件
        /// </summary>
        /// <param name="p_FileFullName"></param>
        public void SaveImage(string p_FileFullName)
        {
            byte[] _ValueBytes = SaveImageToTGA();
            if (_ValueBytes != null) File.WriteAllBytes(p_FileFullName, _ValueBytes);
        }

        public double Sample(double x, double y)
        {
            double ax = x - Math.Floor(x);
            double ay = y - Math.Floor(y);
            double bx = 1 - ax;
            double by = 1 - ay;

            int x0 = (int)Math.Floor(x);
            int y0 = (int)Math.Floor(y);

            if (x0 >= this.m_ImageWidth - 1)
            {
                x0 = x0 - 1;
            }
            if (y0 >= this.m_ImageHeight - 1)
            {
                y0 = y0 - 1;
            }

            int x1 = x0 + 1;
            int y1 = y0 + 1;

            Clamp(ref x0, ref y0);
            Clamp(ref x1, ref y1);

            double x0y0 = ToGray(x0, y0);
            double x0y1 = ToGray(x0, y1);
            double x1y0 = ToGray(x1, y0);
            double x1y1 = ToGray(x1, y1);

            return by * (bx * x0y0 + ax * x1y0) + ay * (bx * x0y1 + ax * x1y1);
        }

        double ToGray(int x, int y)
        {

            return Pixels[x + m_ImageWidth * y];
        }

        public void Clamp(ref int x, ref int y)
        {
            x = Math.Max(0, Math.Min(this.m_ImageWidth - 1, x));
            y = Math.Max(0, Math.Min(this.m_ImageHeight - 1, y));
        }
    }
}
