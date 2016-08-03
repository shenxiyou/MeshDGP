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
//		This C# plugin is ported from: $(MAYADIR)\devkit\ik2Bsolver.

//////////////////////////////////////////////////////////////////
//
// ik2Bsolver: IK 2 Bone Solver
//
// This IK solver solves for 2 bones with
// rotate plane capability.
//
// To create the solver, load the plugin, and then type
// the following in the command window:
//
//   createNode -n ik2BsolverCSharp ik2BsolverCSharp;
//
// To use the solver, create two bones using the Joint Tool.
// Then either use the IK Handle Tool, 
// or type the following in the command window:
//
//   ikHandle -sol ik2BsolverCSharp -sj joint1 -ee joint3;
//
// Moving the handle will cause the IK solver to solve.
//
// For convenience, the command "addIK2BsolverCallbacks"
// will set up callbacks to recreate the ik2Bsolver after a 
// File->New or File->Open, so that the solver will 
// appear to be persistent.

using System;
using System.Runtime.InteropServices;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.addMyIK2BsolverCallbacks), "addMyIK2BsolverCallbacksCSharp")]
[assembly: MPxNodeClass(typeof(MayaNetPlugin.ik2Bsolver), "ik2BsolverCSharp", 0x00081063, NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kIkSolverNode)]


namespace MayaNetPlugin
{
    class ik2Bsolver : MPxIkSolverNode, IMPxNode
    {
        const double kEpsilon = 1.0e-5;

        public override void postConstructor()
        {
            rotatePlane = true;
        }

        public override string solverTypeName()
        {
            return "ik2BsolverCSharp";
        }

        public override void doSolve()
        {
            MIkHandleGroup handle_group = handleGroup;
            if (handle_group == null)
                throw new InvalidOperationException("Invalid handle group");

            MObject handle = handle_group.handle(0);
	        MDagPath handlePath = MDagPath.getAPathTo(handle);
	        MFnIkHandle handleFn = new MFnIkHandle(handlePath);

            //Effector
            //
            MDagPath effectorPath = new MDagPath();
            handleFn.getEffector(effectorPath);
            MFnIkEffector effectorFn = new MFnIkEffector(effectorPath);

            effectorPath.pop();
            MFnIkJoint midJoinFn = new MFnIkJoint(effectorPath);

            // Start Joint
	        //
	        MDagPath startJointPath = new MDagPath();
	        handleFn.getStartJoint(startJointPath);
	        MFnIkJoint startJointFn = new MFnIkJoint(startJointPath);

            // Preferred angles
	        //
	        double [] startJointPrefAngle = new double[3];
            double[]  midJointPrefAngle = new double[3];

            startJointFn.getPreferedAngle(startJointPrefAngle);
            midJoinFn.getPreferedAngle(midJointPrefAngle);

            // Set to preferred angles
            //
            startJointFn.setRotation(startJointPrefAngle, startJointFn.rotationOrder);

            midJoinFn.setRotation(midJointPrefAngle, midJoinFn.rotationOrder);

            MPoint handlePos = handleFn.rotatePivot(MSpace.Space.kWorld);
            AwPoint awHandlePos = new AwPoint(handlePos.x, handlePos.y, handlePos.z, handlePos.w);

            MPoint effectorPos = effectorFn.rotatePivot(MSpace.Space.kWorld);
            AwPoint awEffectorPos = new AwPoint(effectorPos.x, effectorPos.y, effectorPos.z, effectorPos.w);

            MPoint midJoinPos = midJoinFn.rotatePivot(MSpace.Space.kWorld);
            AwPoint awMidJoinPos = new AwPoint(midJoinPos.x, midJoinPos.y, midJoinPos.z, midJoinPos.w);

            MPoint startJointPos = startJointFn.rotatePivot(MSpace.Space.kWorld);
            AwPoint awStartJointPos = new AwPoint(startJointPos.x, startJointPos.y, startJointPos.z, startJointPos.w);

            AwVector poleVector = poleVectorFromHandle(handlePath);
            MMatrix m = handlePath.exclusiveMatrix;
            AwMatrix awM = new AwMatrix();
            awM.setMatrix(m);
            poleVector = poleVector.mulMatrix(awM);
            double twistValue = twistFromHandle(handlePath);

            AwQuaternion qStart = new AwQuaternion();
            AwQuaternion qMid = new AwQuaternion();

            solveIK(awStartJointPos, awMidJoinPos, awEffectorPos, awHandlePos,
                poleVector, twistValue, qStart, qMid);

            MQuaternion mid = new MQuaternion(qMid.x, qMid.y, qMid.z, qMid.w);
            MQuaternion start = new MQuaternion(qStart.x, qStart.y, qStart.z, qStart.w);

            midJoinFn.rotateBy(mid, MSpace.Space.kWorld);
            startJointFn.rotateBy(start, MSpace.Space.kWorld);

            return;
        }

        double twistFromHandle(MDagPath handlePath)
        // This method returns the twist of the IK handle.
        //
        {
	        MFnIkHandle handleFn = new MFnIkHandle(handlePath);
	        MPlug twistPlug = handleFn.findPlug("twist");
	        double twistValue = 0.0;
	        twistPlug.getValue(twistValue);
	        return twistValue;
        }

        void solveIK( AwPoint startJointPos,
			  AwPoint midJointPos,
			  AwPoint effectorPos,
			  AwPoint handlePos,
			  AwVector poleVector,
			  double twistValue,
			  AwQuaternion qStart,
              AwQuaternion qMid)
        // This is method that actually computes the IK solution.
        //
        {
            // vector from startJoint to midJoint
            AwVector vector1 = midJointPos.sub(startJointPos);
            // vector from midJoint to effector
            AwVector vector2 = effectorPos.sub(midJointPos);
            // vector from startJoint to handle
            AwVector vectorH = handlePos.sub(startJointPos);
            // vector from startJoint to effector
            AwVector vectorE = effectorPos.sub(startJointPos);

            // lengths of those vectors
            double length1 = vector1.length();
            double length2 = vector2.length();
            double lengthH = vectorH.length();

            double d = vector1.mul(vectorE) / vectorE.mul(vectorE);
            AwVector vectorO = vector1.sub(vectorE.mul(d));

            //////////////////////////////////////////////////////////////////
            // calculate q12 which solves for the midJoint rotation
            //////////////////////////////////////////////////////////////////
            // angle between vector1 and vector2
            double vectorAngle12 = vector1.angle(vector2);

            // vector orthogonal to vector1 and 2
            AwVector vectorCross12 = vector1.crossProduct(vector2);
            double lengthHsquared = lengthH * lengthH;

            // angle for arm extension 
	        double cos_theta = 
		        (lengthHsquared - length1*length1 - length2*length2)
		        /(2*length1*length2);
	        
            if (cos_theta > 1) 
		        cos_theta = 1;
	        else if (cos_theta < -1) 
		        cos_theta = -1;

            double theta = Math.Acos(cos_theta);

            AwQuaternion q12 = new AwQuaternion(theta - vectorAngle12, vectorCross12);

            //////////////////////////////////////////////////////////////////
	        // calculate qEH which solves for effector rotating onto the handle
	        //////////////////////////////////////////////////////////////////
	        // vector2 with quaternion q12 applied
	        vector2 = vector2.rotateBy(q12);
	        // vectorE with quaternion q12 applied
	        vectorE = vector1.add(vector2);
	        // quaternion for rotating the effector onto the handle
	        AwQuaternion qEH = new AwQuaternion(vectorE, vectorH);

            // calculate qNP which solves for the rotate plane
            //////////////////////////////////////////////////////////////////
            // vector1 with quaternion qEH applied
            vector1 = vector1.rotateBy(qEH);
            if (vector1.isParallel(vectorH,AwMath.kDoubleEpsilon))
                // singular case, use orthogonal component instead
                vector1 = vectorO.rotateBy(qEH);

            AwQuaternion qNP = new AwQuaternion();
            if (!poleVector.isParallel(vectorH, AwMath.kDoubleEpsilon) && (lengthHsquared != 0))
            {
                double temp = poleVector.mul(vectorH) / lengthHsquared;
                AwVector vectorN = vector1.sub(vectorH.mul(temp));

                AwVector vectorP = poleVector.sub(vectorH.mul(vector1.mul(vectorH) / lengthHsquared));

                double dotNP = (vectorN.mul(vectorP)) / (vectorN.length() * vectorP.length());

                if (Math.Abs(dotNP + 1.0) < kEpsilon) 
                {
			        // singular case, rotate halfway around vectorH
			        AwQuaternion qNP1 = new AwQuaternion(AwMath.kPi, vectorH);
			        qNP = qNP1;
		        }
		        else 
                {
			        AwQuaternion qNP2 = new AwQuaternion(vectorN, vectorP);
			        qNP = qNP2;
		        }
            }

            //////////////////////////////////////////////////////////////////
	        // calculate qTwist which adds the twist
	        //////////////////////////////////////////////////////////////////
	        AwQuaternion qTwist = new AwQuaternion(twistValue, vectorH);

	        // quaternion for the mid joint
	        qMid = q12;	
	        // concatenate the quaternions for the start joint
            AwQuaternion qTemp = qEH.mul(qNP);
            qStart = qTemp.mul(qTwist);
        }

        AwVector poleVectorFromHandle( MDagPath handlePath)
        {
           
            MFnIkHandle handleFn = new MFnIkHandle(handlePath);
            MPlug pvxPlug = handleFn.findPlug("pvx");
            MPlug pvyPlug = handleFn.findPlug("pvy");
            MPlug pvzPlug = handleFn.findPlug("pvz");
            double pvxValue, pvyValue, pvzValue;
            pvxValue=pvyValue=pvzValue=0;
            pvxPlug.getValue(pvxValue);
            pvyPlug.getValue(pvyValue);
            pvzPlug.getValue(pvzValue);
            AwVector poleVector = new AwVector(pvxValue, pvyValue, pvzValue);
            return poleVector;
        }
    }

    class addMyIK2BsolverCallbacks : MPxCommand, IMPxCommand
    {
        static private void createIK2BsolverAfterNew(object sender, MBasicFunctionArgs arg)
        // This method creates the ik2Bsolver after a File->New.
        //
        {
	        MSelectionList selList = new MSelectionList();
	        MGlobal.getActiveSelectionList( selList );
            MGlobal.executeCommand("createNode -n ik2BsolverCSharp ik2BsolverCSharp");
	        MGlobal.setActiveSelectionList( selList );
        }

        static private void createIK2BsolverAfterOpen(object sender, MBasicFunctionArgs arg)
        // This method creates the ik2Bsolver after a File->Open
        // if the ik2Bsolver does not exist in the loaded file.
        //
        {
	        MSelectionList selList = new MSelectionList();
            MGlobal.getSelectionListByName("ik2BsolverCSharp", selList);
	        if (selList.length == 0) 
            {
		        MGlobal.getActiveSelectionList( selList );
                MGlobal.executeCommand("createNode -n ik2BsolverCSharp ik2BsolverCSharp");
		        MGlobal.setActiveSelectionList( selList );
	        }
        }

        override public void doIt(MArgList args)
        {
            MSceneMessage.AfterNew += addMyIK2BsolverCallbacks.createIK2BsolverAfterNew;
            MSceneMessage.AfterOpen += addMyIK2BsolverCallbacks.createIK2BsolverAfterOpen;

	        return;
        }
    }

   
}
