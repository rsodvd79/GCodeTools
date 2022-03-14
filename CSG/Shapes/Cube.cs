﻿namespace CSG.Shapes
{
    using System;
    using System.Numerics;
    using System.Runtime.Serialization;

    [Serializable]
    public class Cube : Shape, IEquatable<Cube>
    {
        private static readonly Vector3[] normals = new []
        {
            new Vector3(+0, +0, +1),
            new Vector3(+0, +0, -1),
            new Vector3(+1, +0, +0),
            new Vector3(-1, +0, +0),
            new Vector3(+0, +1, +0),
            new Vector3(+0, -1, +0),
        };

        public Vector3 Position { get; set; }

        public Vector3 Size { get; set; }

        public Cube(Vector3? position = null, Vector3? size = null)
        {
            this.Position = position ?? Vector3.Zero;
            this.Size = size ?? Vector3.One;
        }

        public Cube(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Position = (Vector3)info.GetValue("position", typeof(Vector3));
            this.Size = (Vector3)info.GetValue("size", typeof(Vector3));
        }

        protected override void OnBuild(IShapeBuilder builder)
        {
            for (int i = 0; i < normals.Length; i++)
            {
                var normal = normals[i];

                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                builder.AddIndex(builder.CurrentVertex + 0);
                builder.AddIndex(builder.CurrentVertex + 1);
                builder.AddIndex(builder.CurrentVertex + 2);

                builder.AddIndex(builder.CurrentVertex + 0);
                builder.AddIndex(builder.CurrentVertex + 2);
                builder.AddIndex(builder.CurrentVertex + 3);

                builder.AddVertex(new Vertex(Position + (((normal - side1 - side2) / 2) * Size), normal, Vector2.Zero));
                builder.AddVertex(new Vertex(Position + (((normal - side1 + side2) / 2) * Size), normal, Vector2.UnitX));
                builder.AddVertex(new Vertex(Position + (((normal + side1 + side2) / 2) * Size), normal, Vector2.One));
                builder.AddVertex(new Vertex(Position + (((normal + side1 - side2) / 2) * Size), normal, Vector2.UnitY));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("position", Position);
            info.AddValue("size", Size);
        }

        public bool Equals(Cube other)
        {
            return base.Equals(other as Shape) && 
                   Position == other.Position && 
                   Size == other.Size;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals(obj as Cube);
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Position.GetHashCode() ^ Size.GetHashCode();
        }
    }
}