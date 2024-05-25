using UnityEngine;

namespace LevAI.Perception
{
    [ExecuteInEditMode]
    public partial class SightSensor
    {
        [SerializeField] private Color _debugColor = new(0, 1.0f, 0, 0.5f);

        private Mesh _mesh;

        private void OnValidate()
        {
            _mesh = CreateMesh();
            _scanInterval = 1.0f / _scanFrequency;
        }

        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color;
            Gizmos.color = _debugColor;

            if (_mesh)
                Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);

            Gizmos.DrawWireSphere(transform.position, _distance);

            Gizmos.color = Color.green;
            foreach (GameObject obj in _gameObjects)
            {
                if (obj)
                    Gizmos.DrawSphere(obj.transform.position, 0.2f);
            }

            Gizmos.color = color;
        }

        Mesh CreateMesh()
        {
            var mesh = new Mesh();

            int segments = Mathf.Max(1, Mathf.RoundToInt(_halfAngle / 5));
            int numTriangles = (segments * segments * 2) + segments * 4;
            int numVertices = numTriangles * 3;
            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 center = Vector3.zero;

            int vert = 0;

            float deltaAngle = _halfAngle * 2 / segments;

            //left
            {
                float currentAngle = -_halfAngle;

                for (int i = 0; i < segments; i++)
                {
                    var bottom = GetFarPoint(currentAngle, _halfAngle);
                    var top = GetFarPoint(currentAngle + deltaAngle, _halfAngle);

                    vertices[vert++] = center;
                    vertices[vert++] = bottom;
                    vertices[vert++] = top;

                    currentAngle += deltaAngle;
                }
            }

            //right
            {
                float currentAngle = -_halfAngle;

                for (int i = 0; i < segments; i++)
                {
                    var bottom = GetFarPoint(currentAngle, -_halfAngle);
                    var top = GetFarPoint(currentAngle + deltaAngle, -_halfAngle);

                    vertices[vert++] = center;
                    vertices[vert++] = top;
                    vertices[vert++] = bottom;

                    currentAngle += deltaAngle;
                }
            }

            //bottom
            {
                float currentAngle = -_halfAngle;

                for (int i = 0; i < segments; i++)
                {
                    var left = GetFarPoint(-_halfAngle, currentAngle);
                    var right = GetFarPoint(-_halfAngle, currentAngle + deltaAngle);

                    vertices[vert++] = center;
                    vertices[vert++] = left;
                    vertices[vert++] = right;

                    currentAngle += deltaAngle;
                }
            }

            //top
            {
                float currentAngle = -_halfAngle;

                for (int i = 0; i < segments; i++)
                {
                    var left = GetFarPoint(_halfAngle, currentAngle);
                    var right = GetFarPoint(_halfAngle, currentAngle + deltaAngle);

                    vertices[vert++] = center;
                    vertices[vert++] = right;
                    vertices[vert++] = left;

                    currentAngle += deltaAngle;
                }
            }

            //far
            {
                float currentAngleX = -_halfAngle;

                for (int i = 0; i < segments; i++)
                {
                    float currentAngle = -_halfAngle;
                    for (int j = 0; j < segments; j++)
                    {
                        var bottomLeft = GetFarPoint(currentAngleX, currentAngle);
                        var bottomRight = GetFarPoint(currentAngleX, currentAngle + deltaAngle);

                        var topLeft = GetFarPoint(currentAngleX + deltaAngle, currentAngle);
                        var topRight = GetFarPoint(currentAngleX + deltaAngle, currentAngle + deltaAngle);

                        vertices[vert++] = bottomLeft;
                        vertices[vert++] = topRight;
                        vertices[vert++] = bottomRight;

                        vertices[vert++] = topRight;
                        vertices[vert++] = bottomLeft;
                        vertices[vert++] = topLeft;

                        currentAngle += deltaAngle;
                    }

                    currentAngleX += deltaAngle;
                }
            }


            for (int i = 0; i < numVertices; i++)
                triangles[i] = i;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;

            Vector3 GetFarPoint(float angleX, float angleY) =>
                Quaternion.Euler(angleX, angleY, 0) * Vector3.forward * _distance;
        }
    }
}