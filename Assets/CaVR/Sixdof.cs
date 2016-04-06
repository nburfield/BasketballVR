using UnityEngine;
using System;
using System.Runtime.Serialization;

[Serializable]
public struct Sixdof
{
	//[SerializableAttribute]
    public Vector3 Position {
        get; set;
    }

	//[SerializableAttribute]
    public Quaternion Rotation {
        get; set;
    }

    public Vector3 Forward {
        get { return Rotation * Vector3.forward; }
    }

    public Vector3 Backward {
        get { return Rotation * Vector3.back; }
    }

    public Vector3 Left {
        get { return Rotation * Vector3.left; }
    }

    public Vector3 Right {
        get { return Rotation * Vector3.right; }
    }

    public Vector3 Up {
        get { return Rotation * Vector3.up; }
    }

    public Vector3 Down {
        get { return Rotation * Vector3.down; }
    }

    public Matrix4x4 Matrix {
        get { return Matrix4x4.TRS(Position, Rotation, new Vector3(1.0f, 1.0f, 1.0f)); }
    }

    public override string ToString() {
        return string.Format("sixdof[pos={0}, rot={1}]", Position, Rotation);
    }

	public static readonly Sixdof Empty = new Sixdof {
		Position = Vector3.zero,
		Rotation = Quaternion.identity
	};
}
