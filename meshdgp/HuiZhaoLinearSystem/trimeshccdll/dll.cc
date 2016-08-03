#ifdef __cplusplus 
#define EXPORT extern "C" __declspec (dllexport)
#else
#define EXPORT __declspec (dllexport)
#endif

#include "TriMesh.h"
#include "TriMesh_algo.h"
#include "XForm.h"
#include "GLCamera.h"
#include "ICP.h"
#include "strutil.h"
#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <string>

using namespace std;
using namespace trimesh;

TriMesh* mesh;

EXPORT void FromObjFile(char* file)
{
	mesh = TriMesh::read(file);
}

EXPORT int GetCurv(float buff[][8])
{
	mesh -> need_curvatures();
	for (int i = 0; i < mesh -> curv1.size(); i++)
	{
		buff[i][0] = mesh -> curv1[i];
		buff[i][1] = mesh -> curv2[i];
		for (int j = 0; j < 3; j++)
		{
			buff[i][j + 2] = mesh -> pdir1[i][j];
			buff[i][j + 5] = mesh -> pdir2[i][j];
		}
	}
	return mesh -> curv1.size();
}

EXPORT int GetDCurv(float buff[][4])
{
	mesh -> need_dcurv();
	for (int i = 0; i < mesh -> dcurv.size(); i++)
	{
		for (int j = 0; j < 4; j++)
		{
			buff[i][j] = mesh -> dcurv[i][j];
		}
	}
	return mesh -> dcurv.size();
}