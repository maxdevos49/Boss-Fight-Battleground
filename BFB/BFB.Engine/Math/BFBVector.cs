using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Math
{
    [Serializable]
    public class BfbVector
    {

        #region Properties

        [UsedImplicitly]
        public float X { get; set; }
        
        [UsedImplicitly]
        public float Y { get; set; }

        [UsedImplicitly]
        public float Magnitude
        {
            get => (float) System.Math.Sqrt(System.Math.Pow(X, 2) + System.Math.Pow(Y, 2));
            set
            {
                Normalize();
                Mult(value);
            }
        }
        
        #endregion

        #region Constructors
        
        public BfbVector()
        {
            X = 0;
            Y = 0;
        }
        
        public BfbVector(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public BfbVector(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }
        
        #endregion
        
        #region Add

        public void Add(BfbVector v)
        {
            X += v.X;
            Y += v.Y;
        }
        
        public static BfbVector Add(BfbVector v1, BfbVector v2)
        {
            return new BfbVector(v1.X + v2.X,v1.Y + v2.Y);
        }
        
        #endregion
        
        #region Sub
        
        public void Sub(BfbVector v)
        {
            X -= v.X;
            Y -= v.Y;
        }
        
        public static BfbVector Sub(BfbVector v1, BfbVector v2)
        {
            return new BfbVector(v1.X - v2.X,v1.Y - v2.Y);
        }
        
        #endregion

        #region Mult

        /**
         * Scales the vector by a given scalar
         */
        [UsedImplicitly]
        public void Mult(float scalar)
        {
            X *= scalar;
            Y *= scalar;
        }
        
        /**
         * Scales the vector by a given scalar
         */
        public static BfbVector Mult(BfbVector v, float scalar)
        {
            return new BfbVector(v.X * scalar,v.Y * scalar);
        }

        #endregion
        
        #region Normalize

        public void Normalize()
        {
            float mag = Magnitude;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (X != 0)
            {
                X /= mag;
            }
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Y != 0)
            {
                Y /= mag;
            }
        }
        
        #endregion
        
        #region Limit

        public void Limit(float magnitude)
        {
            if(Magnitude > magnitude)
                Magnitude = magnitude;
        }
        
        #endregion
        
        #region GetRadians

        public float GetRadians()
        {
            return (float)System.Math.Atan2(Y, X);
        }
        
        #endregion
        
        #region GetDegrees

        public float GetDegrees()
        {
            return (float) (GetRadians() * (180 / System.Math.PI));
        }
        
        #endregion

        #region ToVector2

        public Vector2 ToVector2()
        {
            return new Vector2(X,Y);
        }
        
        #endregion
        
    }
}