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
//	This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\XmlGeometryCacheDesc
//
// Description:
//	This plug-in provides an example of the use of MPxCacheFormat.
//
//	In this example, the cache files are written in XML format.
//	Also, the handling of the description file is overridden.
//
//	This example extends the XmlGeometryCache.cs example.
//
//	For more detail, please look at "Geometry caching overview" in Maya API Documentation
//	and the accompanying MEL script.

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

using Autodesk.Maya.OpenMaya;


[assembly: MPxCacheFormatClass(typeof(MayaNetPlugin.XmlCacheFormatDesc), MayaNetPlugin.XmlCacheFormatDesc.fCacheFormatName)]

namespace MayaNetPlugin
{
	public class XmlCacheFormatDesc : MPxCacheFormat
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
		const string Autodesk_Cache_File = "Autodesk_Cache_File";
		const string cacheType_Type = "cacheType-Type";
		const string OneFilePerFrame = "OneFilePerFrame";
		const string OneFile = "OneFile";
		const string cacheType_Format = "cacheType-Format";
		const string cacheTimePerFrame_TimePerFrame = "cacheTimePerFrame-TimePerFrame";
		const string cacheVersion_Version = "cacheVersion-Version";
		const string extra = "extra";
		const string Channels = "Channels";
		const string ChannelNameTag = "ChannelName";
		const string ChannelTypeTag = "ChannelType";
		const string ChannelInterpretation = "ChannelInterpretation";
		const string SamplingType = "SamplingType";
		const string Double = "Double";
		const string DoubleArray = "DoubleArray";
		const string DoubleVectorArray = "DoubleVectorArray";
		const string Int32Array = "Int32Array";
		const string FloatArray = "FloatArray";
		const string FloatVectorArray = "FloatVectorArray";
		const string Regular = "Regular";
		const string Irregular = "Irregular";
		const string SamplingRate = "SamplingRate";
		const string StartTime = "StartTime";
		const string EndTime = "EndTime";

		XmlDocument myXmlDoc;
		string myFileName;
		MPxCacheFormat.FileAccessMode myFileMode;

		XmlNode myCurrentChunkNode;
		XmlNode myCurrentChanelNode;
		int myCurrentChanelIndex = -1;
		XmlNode myRootNode;

		const string fExtension = "mc_csharp_no_desc";			// For files on disk
		public const string fCacheFormatName = "xml_csharp_no_desc";	// For presentation in GUI

		public override void close()
		{
			if (myXmlDoc != null)
			{
                if (myFileMode == MPxCacheFormat.FileAccessMode.kWrite||
                    myFileMode == MPxCacheFormat.FileAccessMode.kReadWrite)
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
                else
                    throw new System.IO.FileNotFoundException("No such file to read", fileName);
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
            return myXmlDoc != null;
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

			return;
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
			return;
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

		public override bool handlesDescription()
		{
			return true;
		}

		public override void readDescription(MCacheFormatDescription description, string descriptionFileLocation, string baseFileName)
		{
			string filename = descriptionFileLocation + baseFileName + ".txt";
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(filename);

			XmlNode root = xmldoc.SelectSingleNode(Autodesk_Cache_File);

			XmlNode cacheTypeNode = root.SelectSingleNode(cacheType_Type);
			string cacheTypeStr = cacheTypeNode.InnerText;
			if (cacheTypeStr.Equals(OneFile))
				description.setDistribution(MCacheFormatDescription.CacheFileDistribution.kOneFile);
			else
				description.setDistribution(MCacheFormatDescription.CacheFileDistribution.kOneFilePerFrame);

			XmlNode timePerFrameNode = root.SelectSingleNode(cacheTimePerFrame_TimePerFrame);
            MTime timePerFrame = new MTime(0.0, MTime.Unit.k6000FPS);
			double dTime = Convert.ToDouble(timePerFrameNode.InnerText);
			timePerFrame.value = dTime;
            description.timePerFrame = timePerFrame;

            XmlNodeList extraNodes = root.SelectNodes(extra);
			foreach (XmlNode node in extraNodes)
			{
				description.addDescriptionInfo(node.InnerText);
			}

			XmlNode channelsNodes = root.SelectSingleNode(Channels);
			XmlNodeList channelNodeList = channelsNodes.SelectNodes(channelTag);

			foreach (XmlNode node in channelNodeList)
			{
				XmlElement elem = (XmlElement)node;
				string channelName = elem.GetAttribute(ChannelNameTag);

				string interPretionStr = elem.GetAttribute(ChannelInterpretation);

				string dataType = elem.GetAttribute(ChannelTypeTag);
				MCacheFormatDescription.CacheDataType cacheDataType = MCacheFormatDescription.CacheDataType.kDouble;
				if (dataType.Equals(Double))
					cacheDataType = MCacheFormatDescription.CacheDataType.kDouble;
				else if (dataType.Equals(DoubleArray))
					cacheDataType = MCacheFormatDescription.CacheDataType.kDoubleArray;
				else if (dataType.Equals(DoubleVectorArray))
					cacheDataType = MCacheFormatDescription.CacheDataType.kDoubleVectorArray;
				else if (dataType.Equals(Int32Array))
					cacheDataType = MCacheFormatDescription.CacheDataType.kInt32Array;
				else if (dataType.Equals(FloatArray))
					cacheDataType = MCacheFormatDescription.CacheDataType.kFloatArray;
				else if (dataType.Equals(FloatVectorArray))
					cacheDataType = MCacheFormatDescription.CacheDataType.kFloatVectorArray;

				string sampleTypeStr = elem.GetAttribute(SamplingType);
				MCacheFormatDescription.CacheSamplingType sample_type = MCacheFormatDescription.CacheSamplingType.kRegular;
				if (sampleTypeStr.Equals(Irregular))
					sample_type = MCacheFormatDescription.CacheSamplingType.kIrregular;


				string sampleRateStr = elem.GetAttribute(SamplingRate);
                MTime sampleRateTime = new MTime(0.0, MTime.Unit.k6000FPS);
				sampleRateTime.value = Convert.ToDouble(sampleRateStr);

				string startTimeStr = elem.GetAttribute(StartTime);
                MTime startTimeTime = new MTime(0.0, MTime.Unit.k6000FPS);
				startTimeTime.value = Convert.ToDouble(startTimeStr);

				string endTimeStr = elem.GetAttribute(EndTime);
                MTime endTimeTime = new MTime(0.0, MTime.Unit.k6000FPS);
				endTimeTime.value = Convert.ToDouble(endTimeStr);

				description.addChannel(channelName, interPretionStr, cacheDataType, sample_type, sampleRateTime, startTimeTime, endTimeTime);
			}

			return;
		}

		public override void writeDescription(MCacheFormatDescription description, string descriptionFileLocation, string baseFileName)
		{
			string fileName = descriptionFileLocation + baseFileName + ".txt";
			XmlDocument xmlDoc = new XmlDocument();
			XmlNode root = xmlDoc.CreateElement(Autodesk_Cache_File);
			xmlDoc.AppendChild(root);

			XmlNode cacheType = xmlDoc.CreateElement(cacheType_Type);
			MCacheFormatDescription.CacheFileDistribution destribution = description.getDistribution();
			switch (destribution)
			{
				case MCacheFormatDescription.CacheFileDistribution.kOneFile:
					cacheType.InnerText = OneFile;
					break;
				case MCacheFormatDescription.CacheFileDistribution.kOneFilePerFrame:
					cacheType.InnerText = OneFilePerFrame;
					break;
			}

			root.AppendChild(cacheType);

			XmlNode cacheFormat = xmlDoc.CreateElement(cacheType_Format);
			cacheFormat.InnerText = fCacheFormatName;
			root.AppendChild(cacheFormat);

            MTime startTime = new MTime(0.0, MTime.Unit.k6000FPS);
            MTime endTime = new MTime(0.0, MTime.Unit.k6000FPS);
			description.getStartAndEndTimes(startTime, endTime);

			XmlNode startTimeNode = xmlDoc.CreateElement(startTimeTag);
			startTimeNode.InnerText = Convert.ToString(startTime.value);
			root.AppendChild(startTimeNode);

			XmlNode endTimeNode = xmlDoc.CreateElement(endTimeTag);
			endTimeNode.InnerText = Convert.ToString(endTime.value);
			root.AppendChild(endTimeNode);

			MTime timePerFrame = description.timePerFrame;
			XmlNode timePerFrameNode = xmlDoc.CreateElement(cacheTimePerFrame_TimePerFrame);
			timePerFrameNode.InnerText = Convert.ToString(timePerFrame.value);
			root.AppendChild(timePerFrameNode);

			XmlNode version = xmlDoc.CreateElement(cacheVersion_Version);
			version.InnerText = "2.0";
			root.AppendChild(version);

			MStringArray info = description.getDescriptionInfo();
			for (int i = 0; i < info.length; i++)
			{
				XmlNode extraNode = xmlDoc.CreateElement(extra);
				extraNode.InnerText = info[i];
				root.AppendChild(extraNode);
			}

			XmlNode channelsNode = xmlDoc.CreateElement(Channels);
			uint channelNum = description.numChannels;
			root.AppendChild(channelsNode);

			for (uint i = 0; i < channelNum; i++)
			{
				string channelName = description.getChannelName(i);
				string interpretation = description.getChannelInterpretation(i);
				MCacheFormatDescription.CacheDataType dataType = description.getChannelDataType(i);
				MCacheFormatDescription.CacheSamplingType sampleType = description.getChannelSamplingType(i);
				MTime sampleRate = description.getChannelSamplingRate(i);
				MTime channelStartTime = description.getChannelStartTime(i);
				MTime channelEndTime = description.getChannelEndTime(i);

				XmlNode channelNode = xmlDoc.CreateElement(channelTag);
				XmlAttribute channelNameAtrr = xmlDoc.CreateAttribute(ChannelNameTag);
				channelNameAtrr.InnerText = channelName;
				channelNode.Attributes.Append(channelNameAtrr);

				string dataTypeStr = "";
				switch (dataType)
				{
					case MCacheFormatDescription.CacheDataType.kDouble:
						dataTypeStr = Double;
						break;
					case MCacheFormatDescription.CacheDataType.kDoubleArray:
						dataTypeStr = DoubleArray;
						break;
					case MCacheFormatDescription.CacheDataType.kDoubleVectorArray:
						dataTypeStr = DoubleVectorArray;
						break;
					case MCacheFormatDescription.CacheDataType.kInt32Array:
						dataTypeStr = Int32Array;
						break;
					case MCacheFormatDescription.CacheDataType.kFloatArray:
						dataTypeStr = FloatArray;
						break;
					case MCacheFormatDescription.CacheDataType.kFloatVectorArray:
						dataTypeStr = FloatVectorArray;
						break;
				}
				XmlAttribute channelTypeAtrr = xmlDoc.CreateAttribute(ChannelTypeTag);
				channelTypeAtrr.InnerText = dataTypeStr;
				channelNode.Attributes.Append(channelTypeAtrr);

				XmlAttribute channelInterpAtrr = xmlDoc.CreateAttribute(ChannelInterpretation);
				channelInterpAtrr.InnerText = interpretation;
				channelNode.Attributes.Append(channelInterpAtrr);

				XmlAttribute channelSampleTypeAtrr = xmlDoc.CreateAttribute(SamplingType);
				switch (sampleType)
				{
					case MCacheFormatDescription.CacheSamplingType.kIrregular:
						channelSampleTypeAtrr.InnerText = Irregular;
						break;
					case MCacheFormatDescription.CacheSamplingType.kRegular:
						channelSampleTypeAtrr.InnerText = Regular;
						break;
				}
				channelNode.Attributes.Append(channelSampleTypeAtrr);

				XmlAttribute sampleRateAtrr = xmlDoc.CreateAttribute(SamplingRate);
				sampleRateAtrr.InnerText = Convert.ToString(sampleRate.value);
				channelNode.Attributes.Append(sampleRateAtrr);

				XmlAttribute startTimeAtrr = xmlDoc.CreateAttribute(StartTime);
				startTimeAtrr.InnerText = Convert.ToString(channelStartTime.value);
				channelNode.Attributes.Append(startTimeAtrr);

				XmlAttribute endTimeAtrr = xmlDoc.CreateAttribute(EndTime);
                endTimeAtrr.InnerText = Convert.ToString(channelEndTime.value);
				channelNode.Attributes.Append(endTimeAtrr);

				channelsNode.AppendChild(channelNode);
			}

			xmlDoc.Save(fileName);
			return;
		}
	}
}
