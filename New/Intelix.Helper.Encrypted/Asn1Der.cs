using System;

namespace Intelix.Helper.Encrypted;

public class Asn1Der
{
	public enum Type
	{
		Sequence = 48,
		Integer = 2,
		OctetString = 4,
		ObjectIdentifier = 6
	}

	public Asn1DerObject Parse(byte[] toParse)
	{
		Asn1DerObject asn1DerObject = new Asn1DerObject();
		for (int i = 0; i < toParse.Length; i++)
		{
			switch ((Type)toParse[i])
			{
			case Type.Sequence:
			{
				byte[] array;
				if (asn1DerObject.Lenght == 0)
				{
					asn1DerObject.Type = Type.Sequence;
					asn1DerObject.Lenght = toParse.Length - (i + 2);
					array = new byte[asn1DerObject.Lenght];
				}
				else
				{
					asn1DerObject.Objects.Add(new Asn1DerObject
					{
						Type = Type.Sequence,
						Lenght = toParse[i + 1]
					});
					array = new byte[toParse[i + 1]];
				}
				int length = ((array.Length > toParse.Length - (i + 2)) ? (toParse.Length - (i + 2)) : array.Length);
				Array.Copy(toParse, i + 2, array, 0, length);
				asn1DerObject.Objects.Add(Parse(array));
				i = i + 1 + toParse[i + 1];
				break;
			}
			case Type.Integer:
			{
				asn1DerObject.Objects.Add(new Asn1DerObject
				{
					Type = Type.Integer,
					Lenght = toParse[i + 1]
				});
				byte[] array = new byte[toParse[i + 1]];
				int length = ((i + 2 + toParse[i + 1] > toParse.Length) ? (toParse.Length - (i + 2)) : toParse[i + 1]);
				Array.Copy(toParse, i + 2, array, 0, length);
				Asn1DerObject[] array3 = asn1DerObject.Objects.ToArray();
				asn1DerObject.Objects[array3.Length - 1].Data = array;
				i = i + 1 + asn1DerObject.Objects[array3.Length - 1].Lenght;
				break;
			}
			case Type.OctetString:
			{
				asn1DerObject.Objects.Add(new Asn1DerObject
				{
					Type = Type.OctetString,
					Lenght = toParse[i + 1]
				});
				byte[] array = new byte[toParse[i + 1]];
				int length = ((i + 2 + toParse[i + 1] > toParse.Length) ? (toParse.Length - (i + 2)) : toParse[i + 1]);
				Array.Copy(toParse, i + 2, array, 0, length);
				Asn1DerObject[] array4 = asn1DerObject.Objects.ToArray();
				asn1DerObject.Objects[array4.Length - 1].Data = array;
				i = i + 1 + asn1DerObject.Objects[array4.Length - 1].Lenght;
				break;
			}
			case Type.ObjectIdentifier:
			{
				asn1DerObject.Objects.Add(new Asn1DerObject
				{
					Type = Type.ObjectIdentifier,
					Lenght = toParse[i + 1]
				});
				byte[] array = new byte[toParse[i + 1]];
				int length = ((i + 2 + toParse[i + 1] > toParse.Length) ? (toParse.Length - (i + 2)) : toParse[i + 1]);
				Array.Copy(toParse, i + 2, array, 0, length);
				Asn1DerObject[] array2 = asn1DerObject.Objects.ToArray();
				asn1DerObject.Objects[array2.Length - 1].Data = array;
				i = i + 1 + asn1DerObject.Objects[array2.Length - 1].Lenght;
				break;
			}
			}
		}
		return asn1DerObject;
	}
}
