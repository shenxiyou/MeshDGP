// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
//
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================

// Description:
//		This is an example of how to define a new XML format for MetaData Structure
//      The assembly attribute StructureSerializeClass will make sure the class is properly registered as a serializer
//		A new XML structure thus can be created using:
//		"dataStructure -format "XML" -asString "<?xml version='1.0' encoding='UTF-8'?><structure name='StructureSample'><member name='BoolSample' type='bool' /><member name='Int32Sample' type='int32' /></structure>";"

using System;
using System.Xml;
using System.Xml.Schema;

using Autodesk.Maya.OpenMaya;

using Autodesk.Maya.MetaData;

[assembly: StructureSerializeClass(typeof(MayaNetPlugin.StructureSerializeXML))]
namespace MayaNetPlugin
{
	public class StructureSerializeXML : StructureSerializer
	{
		private static string formatName = "XML";				//! Name of the XML format type
		private static string structureTag = "structure";		//! Main tag id
		private static string memberTag = "member";				//! Structure member tag id
		private static string structureNameAttribute = "name";	//! Structure name tag id
		private static string memberNameAttribute = "name";		//! Member name tag id
		private static string memberDimAttribute = "dim";		//! Member dim tag id
		private static string memberTypeAttribute = "type";		//! Member type tag id

		public override string formatType()
		{
			return formatName;
		}

		public override Structure read(MIStream cSrc, ref string errors)
		{
			int errCount = 0;
			Structure newStructure = null;
			errors = "";

			uint size = MStreamUtils.getLength(cSrc);
			string myString = "";

			// The last argument is true since we don't want to stop reading a stream when a WhiteSpace is encountered.
			MStreamUtils.readCharBuffer(cSrc, out myString, size, true);

			XmlReader reader = XmlReader.Create(new System.IO.StringReader (myString));
			string elementName = "";
			while (reader.Read())
			{
				// Skip anything unrecognized, for maximum flexibility
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				elementName = reader.Name;

				// Root must be a <structure> tag
				if (elementName == structureTag)
				{
					bool isStrucNameFound = reader.MoveToAttribute (structureNameAttribute);
					if (isStrucNameFound)
					{
						string structureName = reader.ReadContentAsString();
						newStructure = new Structure( structureName );
					}
					else
					{
						string msgFmt = MStringResource.getString(MetaDataRegisterMStringResources.kStructureXMLStructureNameNotFound);
						IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;
						int lineNumber = xmlInfo.LineNumber;
						string errorMsg = String.Format(msgFmt, lineNumber.ToString());

						errors += errorMsg;
						errCount++;
						continue;
					}
				}

				if (newStructure == null)
				{
					continue;
				}

				// All children must all be <member> tags
				if (elementName == memberTag)
				{
					uint memberDim = 1;
					string memberName = "";
					string memberType = "";

					bool isMemberNameFound = reader.MoveToAttribute (memberNameAttribute);
					if (isMemberNameFound)
					{
						memberName = reader.ReadContentAsString();
					}
					else
					{
						string msgFmt = MStringResource.getString(MetaDataRegisterMStringResources.kStructureXMLMemberNameNotFound);
						IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;
						int lineNumber = xmlInfo.LineNumber;
						string errorMsg = String.Format(msgFmt, lineNumber.ToString());
						errors += errorMsg;
						continue;
					}
					bool isMemberTypeFound = reader.MoveToAttribute (memberTypeAttribute);
					if (isMemberTypeFound)
					{
						memberType = reader.ReadContentAsString();
					}
					else
					{
						string msgFmt = MStringResource.getString(MetaDataRegisterMStringResources.kStructureXMLMemberTypeNotFound);
						IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;
						int lineNumber = xmlInfo.LineNumber;
						string errorMsg = String.Format(msgFmt, lineNumber.ToString());
						errors += errorMsg;
						continue;
					}

                    bool isMemberDimFound = reader.MoveToAttribute(memberDimAttribute);
                    if (isMemberDimFound)
                        memberDim = (uint)reader.ReadContentAsInt();
                    else
                        memberDim = 1;

                    newStructure.addMember( Member.typeFromName(memberType),memberDim, memberName );
				}


			}

			// If there were errors any structure created will be incorrect so pass
			// back nothing rather than bad data.
			if( errCount > 0 )
				newStructure = null;

			return newStructure;
		}

		public override int write(Structure dataToWrite, MOStream cDst)
		{
			MStreamUtils.writeCharBuffer(cDst, "<?xml version='1.0' encoding='UTF-8'?>\n");

			// Start with the main structure tag containing the name
			string rootString = "<" + structureTag + " " + structureNameAttribute + "='" + dataToWrite.name + "'>\n";
			MStreamUtils.writeCharBuffer(cDst,rootString);

			// Write out each structure member in its own tag
            for (StructureIterator iterator = dataToWrite.begin(); iterator != dataToWrite.end(); iterator = iterator.next())
			{
				string memberString = "    <" + memberTag + " " + memberNameAttribute + "='" + iterator.memberName() + "'"
										+ " " + memberTypeAttribute + "='" + Member.typeName(iterator.memberType()) + "'";

                if (iterator.memberLength() != 1)
				{
					memberString = memberString + " " + memberDimAttribute + "='" + iterator.memberLength() + "'";
				}

				memberString += "/>\n";

				MStreamUtils.writeCharBuffer(cDst,memberString);
			}

			rootString = "</" + structureTag + ">";
			MStreamUtils.writeCharBuffer(cDst,rootString);
			return 0;
		}

		public override void getFormatDescription(MOStream info)
		{
			string msgPre = MStringResource.getString(MetaDataRegisterMStringResources.kStructureXMLInfoPre);
			string msgPost = MStringResource.getString(MetaDataRegisterMStringResources.kStructureXMLInfoPost);

			MStreamUtils.writeChar(info,msgPre[0]);

            for (uint i = (uint)Member.eDataType.kFirstType; i < (uint)Member.eDataType.kLastType; ++i)
			{
                if (i != (uint)Member.eDataType.kFirstType)
				{
					MStreamUtils.writeCharBuffer(info,", ");
				}
			}

			MStreamUtils.writeChar(info,msgPost[0]);
		}

	}
}
