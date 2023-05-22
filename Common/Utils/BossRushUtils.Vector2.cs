﻿using System;
using Terraria;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        public static Vector2 LimitedVelocity(this Vector2 velocity, float limited)
        {
            GetAbsoluteVectorNormalized(velocity, limited, out float X, out float Y);
            velocity.X = Math.Clamp(velocity.X, -X, X);
            velocity.Y = Math.Clamp(velocity.Y, -Y, Y);
            return velocity;
        }
        public static Vector2 LimitedPosition(this Vector2 position, Vector2 position2, float limited)
        {
            position.X = Math.Clamp(position.X, -limited + position2.X, limited + position2.X);
            position.Y = Math.Clamp(position.Y, -limited + position2.Y, limited + position2.Y);
            return position;
        }
        /// <summary>
        /// Take a bool and return a int number base on true or false
        /// </summary>
        /// <param name="Num"></param>
        /// <returns>Return 1 if true
        /// <br/>Otherwise return 0</returns>
        public static int BoolZero(this bool Num) => Num ? 1 : 0;
        public static int BoolOne(this bool Num) => Num ? 1 : -1;
        public static Vector2 NextVector2RectangleEdge(this UnifiedRandom r, float RectangleWidthHalf, float RectangleHeightHalf)
        {
            float X = r.NextFloat(-RectangleWidthHalf, RectangleWidthHalf);
            float Y = r.NextFloat(-RectangleHeightHalf, RectangleHeightHalf);
            bool Randomdecider = r.NextBool();
            Vector2 RandomPointOnEdge = new Vector2(X * Randomdecider.BoolZero(), Y * (!Randomdecider).BoolZero());
            if (RandomPointOnEdge.X == 0)
            {
                RandomPointOnEdge.X = RectangleWidthHalf;
            }
            else
            {
                RandomPointOnEdge.Y = RectangleHeightHalf;
            }
            return RandomPointOnEdge * r.NextBool().BoolOne();
        }
        public static bool Vector2WithinRectangle(this Vector2 position, float X, float Y, Vector2 Center)
        {
            Vector2 positionNeedCheck1 = new Vector2(Center.X + X, Center.Y + Y);
            Vector2 positionNeedCheck2 = new Vector2(Center.X - X, Center.Y - Y);
            if (position.X < positionNeedCheck1.X && position.X > positionNeedCheck2.X && position.Y < positionNeedCheck1.Y && position.Y > positionNeedCheck2.Y)
            { return true; }//higher = -Y, lower = Y
            return false;
        }
        public static bool Vector2TouchLine(float pos1, float pos2, float CenterY)
        {
            if (pos1 < (CenterY + pos2) && pos1 > (CenterY - pos2))
            { return true; }//higher = -Y, lower = Y
            return false;
        }
        public static bool IsLimitReached(this Vector2 velocity, float limited)
        {
            if (velocity.X < limited && velocity.X > -limited &&
                velocity.Y < limited && velocity.Y > -limited)
            {
                return false;
            }
            return true;
        }
        [Obsolete("Not working", true)]
        public static bool ReachedLimited(this Vector2 velocity, float limited)
        {
            GetAbsoluteVectorNormalized(velocity, limited, out float X, out float Y);
            if (Math.Abs(velocity.X) >= X) return true;
            if (Math.Abs(velocity.Y) >= Y) return true;
            return false;
        }
        public static void GetAbsoluteVectorNormalized(Vector2 velocity, float limit, out float X, out float Y)
        {
            Vector2 newVelocity = velocity.SafeNormalize(Vector2.Zero) * limit;
            X = Math.Abs(newVelocity.X);
            Y = Math.Abs(newVelocity.Y);
        }
        public static Vector2 Vector2DistributeEvenly(this Vector2 velocity, float ProjectileAmount, float rotation, int i)
        {
            if (ProjectileAmount > 1)
            {
                rotation = MathHelper.ToRadians(rotation);
                return velocity.RotatedBy(MathHelper.Lerp(rotation * .5f, rotation * -.5f, i / (ProjectileAmount - 1f)));
            }
            return velocity;
        }
        public static Vector2 NextVector2RotatedByRandom(this Vector2 velocity, float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ToRadians);
            return velocity.RotatedByRandom(rotation);
        }
        public static Vector2 NextVector2Spread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
            ToRotateAgain.Y += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
            return ToRotateAgain;
        }
        public static Vector2 Vector2SmallestInList(List<Vector2> flag)
        {
            for (int i = 0; i < flag.Count;)
            {
                Vector2 vector2 = flag[i];
                for (int l = i + 1; l < flag.Count; ++l)
                {
                    if (vector2.LengthSquared() > flag[l].LengthSquared())
                    {
                        vector2 = flag[l];
                    }
                }
                return vector2;
            }
            return Vector2.Zero;
        }
    }
}
