using System;
using System.Linq;
using System.Numerics;

namespace Hav3Fun
{
    internal class Calculations
    {
        internal static float GetDistance3D(Vector3 playerPosition, Vector3 enemyPosition) =>
            Convert.ToSingle(Math.Sqrt(Math.Pow(enemyPosition.X - playerPosition.X, 2f) +
            Math.Pow(enemyPosition.Y - playerPosition.Y, 2f) + Math.Pow(enemyPosition.Z - playerPosition.Z, 2f)));

        internal static double GetPlayerDistance(Vector3 myLoc, Vector3 enemyLoc) =>
            Math.Sqrt(Math.Pow(enemyLoc.X - myLoc.X, 2) +
            Math.Pow(enemyLoc.Y - myLoc.Y, 2) + Math.Pow(enemyLoc.Z - myLoc.Z, 2)) * 0.01905f;

        internal static Vector3 GetBonePos(int entity, int targetBone)
        {
            int boneMatrix = Memory.Read<int>(entity + Offset.m_dwBoneMatrix);

            if (boneMatrix == 0)
                return Vector3.Zero;

            float[] position = Memory.ReadMatrix<float>(boneMatrix + 0x30 * targetBone + 0x0C, 9);

            if (!position.Any())
                return Vector3.Zero;

            return new Vector3(position[0], position[4], position[8]);
        }

        internal static Vector3 SmoothAim(Vector3 viewAngle, Vector3 destination, float smoothAmount)
        {
            Vector3 smoothedAngle = destination - viewAngle;

            smoothedAngle /= smoothAmount;
            smoothedAngle += viewAngle;

            smoothedAngle = NormalizeAngle(smoothedAngle);
            smoothedAngle = ClampAngle(smoothedAngle);

            return smoothedAngle;
        }

        internal static Vector3 ClampAngle(Vector3 angle)
        {
            while (angle.Y > 180) angle.Y -= 360;
            while (angle.Y < -180) angle.Y += 360;

            if (angle.X > 89.0f) angle.X = 89.0f;
            if (angle.X < -89.0f) angle.X = -89.0f;

            angle.Z = 0f;

            return angle;
        }

        internal static Vector3 NormalizeAngle(Vector3 angle)
        {
            if (angle.X < -180.0f) angle.X += 360.0f;
            if (angle.X > 180.0f) angle.X -= 360.0f;

            if (angle.Y < -180.0f) angle.Y += 360.0f;
            if (angle.Y > 180.0f) angle.Y -= 360.0f;

            if (angle.Z < -180.0f) angle.Z += 360.0f;
            if (angle.Z > 180.0f) angle.Z -= 360.0f;

            return angle;
        }

        internal static Vector3 CalcAngle(Vector3 playerPosition, Vector3 enemyPosition, Vector3 aimPunch, Vector3 vecView, float RcsX, float RcsY)
        {
            Vector3 delta = new Vector3(playerPosition.X - enemyPosition.X, playerPosition.Y - enemyPosition.Y, (playerPosition.Z + vecView.Z) - enemyPosition.Z);

            Vector3 Vector = Vector3.Zero;
            Vector.X = Convert.ToSingle(Math.Atan(delta.Z / Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y))) * 57.295779513082f - aimPunch.X * RcsX;
            Vector.Y = Convert.ToSingle(Math.Atan(delta.Y / delta.X)) * (180.0f / (float)Math.PI) - aimPunch.Y * RcsY;
            Vector.Z = 0;

            if (delta.X >= 0.0)
                Vector.Y += 180f;

            Vector = NormalizeAngle(Vector);
            Vector = ClampAngle(Vector);

            return Vector;
        }
    }
}