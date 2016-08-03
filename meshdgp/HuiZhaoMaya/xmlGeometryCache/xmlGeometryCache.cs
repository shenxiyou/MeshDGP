// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Note:
//	This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\XmlGeometryCache
//
// Description:
//	This plug-in provides an example of the use of MPxCacheFormat.
//
//	In this example, the cache files are written in XML format.
//
//	For more detail, please look at "Geometry caching overview" in Maya API Documentation and the
//	accompanying MEL script.

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

using Autodesk.Maya.OpenMaya;


[assembly: MPxCacheFormatClass(typeof(MayaNetPlugin.XmlCacheFormat), MayaNetPlugin.XmlCacheFormat.fCacheFormatName)]

namespace MayaNetPlugin
{
	public class XmlCacheFormat : MPxCacheFormat
	{
		const string rootTag = "root";
		const string cacheTag = "awGeoCache";
		const string startTimeTag = "startTime";
		const string endTimeTag = "endTime";
		const string versionTag = "version";
		const string timeTag = "time";
		const string sizeTag = "size";
		const string nameTag = "name";
		const string intTag = "integer32";
		const string doubleArrayTag = "doubleArray";
		const string floatArrayTag = "floatArray";
		const string doubleVectorArrayTag = "doubleVectorArray";
		const string floatVectorArrayTag = "floatVectorArray";
		const string channelTag = "channel";
		const string chunkTag = "chunk";

		XmlDocument myXmlDoc;
		string myFileName;
		MPxCacheFormat.FileAccessMode myFileMode;

		XmlNode myCurrentChunkNode;
		XmlNode myCurrentChanelNode;
		int myCurrentChanelIndex = -1;
		XmlNode myRootNode;

		const string fExtension = "mc_csharp";			// For files on disk
		public const string fCacheFormatName = "xml_csharp";	// For presentation in GUI

		public override void close()
		{
			if (myXmlDoc != null)
			{
				myXmlDoc.Save(myFileName);
				myXmlDoc = null;
			}
		}

		public override void open(string fileName, MPxCacheFormat.FileAccessMode mode)
		{
			Trace.Assert(fileName.Length > 0);

			myFileName = fileName;
			myFileMode = mode;

			myXmlDoc = new XmlDocument();


			if (mode == MPxCacheFormat.FileAccessMode.kReadWrite ||
				mode == MPxCacheFormat.FileAccessMode.kRead)
			{
				if (File.Exists(myFileName))
				{
					myXmlDoc.Load(myFileName);
				}
			}
			else
			{
				myXmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
				myRootNode = myXmlDoc.CreateElement(rootTag);
				myXmlDoc.AppendChild(myRootNode);
			}

			if (mode == MPxCacheFormat.FileAccessMode.kRead)
			{
				readHeader();
			}
		}

		public override bool isValid()
		{
            return (myXmlDoc != null) ? true : false;
		}

		public override void readHeader()
		{
			string xmlPath = "/" + rootTag + "/" + cacheTag;
			XmlNode headNode = myXmlDoc.SelectSingleNode(xmlPath);

			XmlNode versionNode = headNode.SelectSingleNode(versionTag);
			string version = versionNode.InnerText;

			XmlNode startTimeNode = headNode.SelectSingleNode(startTimeTag);
			string startTime = startTimeNode.InnerText;

			XmlNode endTimeNode = headNode.SelectSingleNode(endTimeTag);
			string endTime = endTimeNode.InnerText;
		}

		public override void rewind()
		{
			return;
		}

		public override void writeHeader(string version, MTime startTime, MTime endTime)
		{

			XmlNode headNode = myXmlDoc.CreateElement(cacheTag);
			myRootNode.AppendChild(headNode);

			XmlNode versionNode = myXmlDoc.CreateElement(versionTag);
			versionNode.InnerText = version;
			headNode.AppendChild(versionNode);

			XmlNode startTimeNode = myXmlDoc.CreateElement(startTimeTag);
			startTimeNode.InnerText = Convert.ToString(startTime.value);
			headNode.AppendChild(startTimeNode);

			XmlNode endTimeNode = myXmlDoc.CreateElement(endTimeTag);
			endTimeNode.InnerText = Convert.ToString(endTime.value);
			headNode.AppendChild(endTimeNode);

			return;
		}

		public override void beginWriteChunk()
		{
			Trace.Assert(myRootNode != null);

			myCurrentChunkNode = myXmlDoc.CreateElement(chunkTag);
			myRootNode.AppendChild(myCurrentChunkNode);
		}

		public override void beginReadChunk()
		{
			string xmlPath = "/" + rootTag + "/" + chunkTag;
			myCurrentChunkNode = myXmlDoc.SelectSingleNode(xmlPath);
			return;
		}

		public override void endWriteChunk()
		{
			myCurrentChunkNode = null;
		}

		public override void endReadChunk()
		{
			myCurrentChunkNode = null;
		}

		public override void writeTime(MTime time)
		{
			if (myCurrentChunkNode != null)
			{
				string strTime = Convert.ToString(time.value);
				XmlNode timeNode = myXmlDoc.CreateElement(timeTag);
				timeNode.InnerText = strTime;
				myCurrentChunkNode.AppendChild(timeNode);
			}

			return;
		}

		public override void readTime(MTime time)
		{
			if (myCurrentChunkNode == null)
			{
				throw new InvalidOperationException("XmlNode cannot be NULL");
			}
			
			XmlNode timeNode = myCurrentChunkNode.SelectSingleNode(timeTag);
			string strTime = timeNode.InnerText;
			time.value = Convert.ToDouble(strTime);
		}

		public override void readNextTime(MTime foundTime)
		{
			MTime readAwTime = new MTime(0.0, MTime.Unit.k6000FPS);
			readTime(readAwTime);
			foundTime.assign(readAwTime);
		}

		public override uint readArraySize()
		{
			Trace.Assert(myCurrentChanelNode != null);

			XmlNode sizeNode = myCurrentChanelNode.SelectSingleNode(sizeTag);
			return Convert.ToUInt32(sizeNode.InnerText);
		}

		public override void readDoubleArray(MDoubleArray array, uint size)
		{
			Trace.Assert(myCurrentChanelNode != null);

			XmlNode doubleArrayNode = myCurrentChanelNode.SelectSingleNode(doubleArrayTag);
			XmlNodeList valueNodeList = doubleArrayNode.SelectNodes("value");
			array.clear();
			array.length = size;
			foreach (XmlNode valueNode in valueNodeList)
			{
				double value = Convert.ToDouble(valueNode.InnerText);
				array.append(value);
			}
			return;
		}

		public override void readFloatArray(MFloatArray array, uint size)
		{
			Trace.Assert(myCurrentChanelNode != null);

			XmlNode floatArrayNode = myCurrentChanelNode.SelectSingleNode(floatArrayTag);
			XmlNodeList valueNodeList = floatArrayNode.SelectNodes("value");
			array.clear();
			array.length = size;
			foreach (XmlNode valueNode in valueNodeList)
			{
				double value = Convert.ToDouble(valueNode.InnerText);
				array.append((float)value);
			}
			return;
		}

		public override void readDoubleVectorArray(MVectorArray array, uint arraySize)
		{
			Trace.Assert(myCurrentChanelNode != null);

			XmlNode doubleVectorArrayNode = myCurrentChanelNode.SelectSingleNode(doubleVectorArrayTag);
			XmlNodeList valueNodeList = doubleVectorArrayNode.SelectNodes("value");
			uint i = 0;
			array.clear();
			array.length = arraySize;
			foreach (XmlNode valueNode in valueNodeList)
			{
				XmlNode xNode = valueNode.SelectSingleNode("x");
				double valueX = Convert.ToDouble(xNode.InnerText);

				XmlNode yNode = valueNode.SelectSingleNode("y");
				double valuey = Convert.ToDouble(yNode.InnerText);

				XmlNode zNode = valueNode.SelectSingleNode("z");
				double valuez = Convert.ToDouble(zNode.InnerText);

				MVector v = new MVector(valueX, valuey, valuez);
				array.set(v, i);
				i++;
			}
			return;
		}

		public override void readFloatVectorArray(MFloatVectorArray array, uint arraySize)
		{
			Trace.Assert(myCurrentChanelNode != null);

			XmlNode floatVectorArrayNode = myCurrentChanelNode.SelectSingleNode(floatVectorArrayTag);
			XmlNodeList valueNodeList = floatVectorArrayNode.SelectNodes("value");
			uint i = 0;
			array.clear();
			array.length = arraySize;

			foreach (XmlNode valueNode in valueNodeList)
			{
				XmlNode xNode = valueNode.SelectSingleNode("x");
				double valueX = Convert.ToDouble(xNode.InnerText);

				XmlNode yNode = valueNode.SelectSingleNode("y");
				double valuey = Convert.ToDouble(yNode.InnerText);

				XmlNode zNode = valueNode.SelectSingleNode("z");
				double valuez = Convert.ToDouble(zNode.InnerText);

				MFloatVector v = new MFloatVector((float)valueX, (float)valuey, (float)valuez);
				array.set(v, i);
				i++;
			}
			return;
		}

		public override void findTime(MTime time, MTime foundTime)
		{
			MTime timeTolerance = new MTime(0.0, MTime.Unit.k6000FPS);
			MTime seekTime = new MTime(time.value, time.unit);
			MTime preTime = seekTime - timeTolerance;
			MTime postTime = seekTime + timeTolerance;

			string xmlPath = "/" + rootTag + "/" + chunkTag;
			XmlNodeList chunkNodeList = myXmlDoc.SelectNodes(xmlPath);
			foreach (XmlNode chunkNode in chunkNodeList)
			{
				XmlNode timeNode = chunkNode.SelectSingleNode(timeTag);
				string strTime = timeNode.InnerText;
				MTime rTime = new MTime(0.0, MTime.Unit.k6000FPS);
				rTime.value = Convert.ToDouble(strTime);
				if (rTime >= preTime && rTime <= postTime)
				{
					foundTime.assign(rTime);
					myCurrentChunkNode = chunkNode;
					return;
				}
			}

			throw new ApplicationException("Failed to find the specified time in the cache");
		}

		public override void writeChannelName(string name)
		{
			if (myCurrentChunkNode != null)
			{
				myCurrentChanelNode = myXmlDoc.CreateElement(channelTag);
				myCurrentChunkNode.AppendChild(myCurrentChanelNode);

				XmlNode nameNode = myXmlDoc.CreateElement(nameTag);
				nameNode.InnerText = name;
				myCurrentChanelNode.AppendChild(nameNode);
			}

			return;
		}

		public override void findChannelName(string name)
		{
			if (myCurrentChunkNode != null)
			{
				XmlNodeList nodeList = myCurrentChunkNode.SelectNodes(channelTag);
				foreach (XmlNode chanelNode in nodeList)
				{
					XmlNode nameNode = chanelNode.SelectSingleNode(nameTag);
					Trace.Assert(nameNode != null);
					if (nameNode.InnerText == name)
					{
						myCurrentChanelNode = chanelNode;
						return;
					}
				}
			}
			throw new ApplicationException("Failed to find the specified channel in the cache");
		}

		public override void readChannelName(out string name)
		{
			Trace.Assert(myCurrentChunkNode != null);

			name = "";
			if (myCurrentChunkNode.ChildNodes.Count <= 0)
			{
				throw new ApplicationException("Current XML node's child nodes cannot be 0.");
			}

			myCurrentChanelIndex++;
			if (myCurrentChanelIndex >= myCurrentChunkNode.ChildNodes.Count)
			{
				myCurrentChanelIndex = -1;
				myCurrentChanelNode = null;
				throw new ApplicationException("Invalid channel index.");
			}

			myCurrentChanelNode = myCurrentChunkNode.ChildNodes[myCurrentChanelIndex];
			XmlNode nameNode = myCurrentChanelNode.SelectSingleNode(nameTag);
			if (nameNode != null)
			{
				name = nameNode.InnerText;
				return;
			}

			throw new ApplicationException("Failed to find the channel name");
		}

		public override void writeDoubleArray(MDoubleArray array)
		{
			if (myCurrentChanelNode != null)
			{
				uint size = array.length;
				XmlNode sizeNode = myXmlDoc.CreateElement(sizeTag);
				sizeNode.InnerText = Convert.ToString(size);
				myCurrentChanelNode.AppendChild(sizeNode);

				XmlNode arrayNode = myXmlDoc.CreateElement(doubleArrayTag);
				myCurrentChanelNode.AppendChild(arrayNode);

				for (int i = 0; i < size; i++)
				{
					string value = array[i].ToString();
					XmlNode valueNode = myXmlDoc.CreateElement("value");
					arrayNode.AppendChild(valueNode);
				}
			}

			return;
		}

		public override void writeFloatArray(MFloatArray array)
		{
			if (myCurrentChanelNode != null)
			{
				uint size = array.length;
				XmlNode sizeNode = myXmlDoc.CreateElement(sizeTag);
				sizeNode.InnerText = Convert.ToString(size);
				myCurrentChanelNode.AppendChild(sizeNode);

				XmlNode arrayNode = myXmlDoc.CreateElement(floatArrayTag);
				myCurrentChanelNode.AppendChild(arrayNode);

				for (int i = 0; i < size; i++)
				{
					string value = array[i].ToString();
					XmlNode valueNode = myXmlDoc.CreateElement("value");
					arrayNode.AppendChild(valueNode);
				}
			}

			return;
		}

		public override void writeDoubleVectorArray(MVectorArray array)
		{
			if (myCurrentChanelNode != null)
			{
				uint size = array.length;
				XmlNode sizeNode = myXmlDoc.CreateElement(sizeTag);
				sizeNode.InnerText = Convert.ToString(size);
				myCurrentChanelNode.AppendChild(sizeNode);

				XmlNode arrayNode = myXmlDoc.CreateElement(doubleVectorArrayTag);
				myCurrentChanelNode.AppendChild(arrayNode);

				for (int i = 0; i < size; i++)
				{
					MVector value = array[i];
					XmlNode valueNode = myXmlDoc.CreateElement("value");
					arrayNode.AppendChild(valueNode);

					XmlNode xValueNode = myXmlDoc.CreateElement("x");
					xValueNode.InnerText = Convert.ToString(value.x);
					valueNode.AppendChild(xValueNode);

					XmlNode yValueNode = myXmlDoc.CreateElement("y");
					yValueNode.InnerText = Convert.ToString(value.y);
					valueNode.AppendChild(yValueNode);

					XmlNode zValueNode = myXmlDoc.CreateElement("z");
					zValueNode.InnerText = Convert.ToString(value.z);
					valueNode.AppendChild(zValueNode);

				}
			}

			return;
		}

		public override void writeFloatVectorArray(MFloatVectorArray array)
		{
			if (myCurrentChanelNode != null)
			{
				uint size = array.length;
				XmlNode sizeNode = myXmlDoc.CreateElement(sizeTag);
				sizeNode.InnerText = Convert.ToString(size);
				myCurrentChanelNode.AppendChild(sizeNode);

				XmlNode arrayNode = myXmlDoc.CreateElement(floatVectorArrayTag);
				myCurrentChanelNode.AppendChild(arrayNode);

				for (int i = 0; i < size; i++)
				{
					MFloatVector value = array[i];
					XmlNode valueNode = myXmlDoc.CreateElement("value");
					arrayNode.AppendChild(valueNode);

					XmlNode xValueNode = myXmlDoc.CreateElement("x");
					xValueNode.InnerText = Convert.ToString(value.x);
					valueNode.AppendChild(xValueNode);

					XmlNode yValueNode = myXmlDoc.CreateElement("y");
					yValueNode.InnerText = Convert.ToString(value.y);
					valueNode.AppendChild(yValueNode);

					XmlNode zValueNode = myXmlDoc.CreateElement("z");
					zValueNode.InnerText = Convert.ToString(value.z);
					valueNode.AppendChild(zValueNode);

				}
			}

			return;
		}

		public override void writeInt32(int value)
		{
			if (myCurrentChanelNode != null)
			{
				XmlNode intNode = myXmlDoc.CreateElement(intTag);
				intNode.InnerText = Convert.ToString(value);
				myCurrentChanelNode.AppendChild(intNode);
			}
			return;
		}

		public override int readInt32()
		{
			if (myCurrentChanelNode != null)
			{
				XmlNode intNode = myCurrentChanelNode.SelectSingleNode(intTag);
				int value = Convert.ToInt32(intNode.InnerText);
				return value;
			}

			return 0;
		}

		public override string extension()
		{
			return fExtension;
		}
	}
}
