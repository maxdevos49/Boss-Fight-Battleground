using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Math
{
    /// <summary>
    /// Represents a serializable vector
    /// </summary>
    [Serializable]
    public class BfbVector
    {

        #region Properties

        /// <summary>
        /// The x component of the vector
        /// </summary>
        public float X { get; set; }
        
        /// <summary>
        /// THe y component of the vector
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The magnitude of the vector
        /// </summary>
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
        
        /// <summary>
        /// Constructs a vector with x and components both zero.
        /// </summary>
        public BfbVector()
        {
            X = 0;
            Y = 0;
        }
        
        /// <summary>
        /// Constructs a vector with given x and y components
        /// </summary>
        /// <param name="x">x component</param>
        /// <param name="y">y comnponent</param>
        public BfbVector(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        /// <summary>
        /// Constructs a vector with a given vector2
        /// </summary>
        /// <param name="v">The given vector2</param>
        public BfbVector(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }
        
        #endregion
        
        #region Add

        /// <summary>
        /// Adds two vector together and mutates the base vector
        /// </summary>
        /// <param name="v"></param>
        public void Add(BfbVector v)
        {
            X += v.X;
            Y += v.Y;
        }
        
        /// <summary>
        /// Adds two vectors together and returns a new vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static BfbVector Add(BfbVector v1, BfbVector v2)
        {
            return new BfbVector(v1.X + v2.X,v1.Y + v2.Y);
        }
        
        #endregion
        
        #region Sub
        
        /// <summary>
        /// Subtracts two vectors and assigns the result to the base vector
        /// </summary>
        /// <param name="v"></param>
        public void Sub(BfbVector v)
        {
            X -= v.X;
            Y -= v.Y;
        }
        
        /// <summary>
        /// Subtracts two vectors and assigns the result to a new vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static BfbVector Sub(BfbVector v1, BfbVector v2)
        {
            return new BfbVector(v1.X - v2.X,v1.Y - v2.Y);
        }
        
        #endregion

        #region Mult

        /// <summary>
        /// Multiplies the vector with a scalar
        /// </summary>
        /// <param name="scalar">The scalar</param>
        [UsedImplicitly]
        public void Mult(float scalar)
        {
            X *= scalar;
            Y *= scalar;
        }
        
        /// <summary>
        ///  Multiplies a vector with a scalar returning a new vector
        /// </summary>
        /// <param name="v"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static BfbVector Mult(BfbVector v, float scalar)
        {
            return new BfbVector(v.X * scalar,v.Y * scalar);
        }

        #endregion
        
        #region Normalize

        /// <summary>
        /// Mutates a vector to become a unit vector
        /// </summary>
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

        /// <summary>
        /// Normalizes the vector to a specified magnitude only if the current magnitude is bigger the the specified magnitude
        /// </summary>
        /// <param name="magnitude"></param>
        public void Limit(float magnitude)
        {
            if(Magnitude > magnitude)
                Magnitude = magnitude;
        }
        
        #endregion
        
        #region GetRadians

        /// <summary>
        /// Returns the angle in radians of the vector
        /// </summary>
        /// <returns></returns>
        public float GetRadians()
        {
            return (float)System.Math.Atan2(Y, X);
        }
        
        #endregion
        
        #region GetDegrees

        /// <summary>
        /// Returns the angle in degrees of the vector
        /// </summary>
        /// <returns></returns>
        public float GetDegrees()
        {
            return (float) (GetRadians() * (180 / System.Math.PI));
        }
        
        #endregion

        #region ToVector2

        /// <summary>
        /// Returns a new Vector2 of the current vector
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVector2()
        {
            return new Vector2(X,Y);
        }
        
        #endregion
        
    }
}