/**
 *  Created by hnim.
 *  Copyright (c) 2017 RedAntz. All rights reserved.
 */
using System.Collections.Generic;
using UnityEngine;
namespace bonbon
{
    public class MathUtils
    {
        public static T random<T>(List<T> pList)
        {
            int count = pList.Count;
            if (count > 1)
            {
                return pList[random(0, count - 1)];
            }
            else
            {
                return pList[0];
            }
        }
        public static bool randomChance(int pRatio, int pTotalRatio)
        {
            return Random.Range(0, pTotalRatio - 1) < pRatio;
        }
        public static bool randomChance(float pRatio, float pTotalRatio)
        {
            return Random.Range(0, pTotalRatio - 1) < pRatio;
        }
        public static int random(int pStart, int pEnd)
        {
            return Random.Range(pStart, pEnd + 1);
        }
        public static float random(float pStart, float pEnd)
        {
            return Random.Range(pStart, pEnd);
        }
        public static bool randomBoolean()
        {
            return random(0, 1) > 0;
        }
        public static float sinRad(float pRad)
        {
            return Mathf.Sin(pRad);
        }
        public static float cosRad(float pRad)
        {
            return Mathf.Cos(pRad);
        }
        public static float sinDeg(float pDeg)
        {
            return Mathf.Sin(degToRad(pDeg));
        }
        public static float cosDeg(float pDeg)
        {
            return Mathf.Cos(degToRad(pDeg));
        }
        public static float tanDeg(float pDeg)
        {
            return Mathf.Tan(degToRad(pDeg));
        }
        public static float degToRad(float pDeg)
        {
            return pDeg * Mathf.Deg2Rad;
        }
        public static float radToDeg(float pRad)
        {
            return pRad * Mathf.Rad2Deg;
        }
        public static float distance(Vector3 pFrom, Vector3 pTo)
        {
            return Vector3.Distance(pFrom, pTo);
        }
        public static float angleDeg(Vector3 pFrom, Vector3 pTo)
        {
            return atanDeg(pTo.y - pFrom.y, pTo.x - pFrom.x);
        }
        public static float atanDeg(float dy, float dx)
        {
            return radToDeg(atanRad(dy, dx));
        }
        public static float atanRad(float dy, float dx)
        {
            return Mathf.Atan2(dy, dx);
        }
        public static int min(int p1, int p2)
        {
            return Mathf.Min(p1, p2);
        }
        public static long min(long p1, long p2)
        {
            return p1 < p2 ? p1 : p2;
        }
        public static float min(float p1, float p2)
        {
            return Mathf.Min(p1, p2);
        }
        public static int max(int p1, int p2)
        {
            return Mathf.Max(p1, p2);
        }
        public static long max(long p1, long p2)
        {
            return p1 > p2 ? p1 : p2;
        }
        public static float max(float p1, float p2)
        {
            return Mathf.Max(p1, p2);
        }
        public static bool isPointInsideEllipse(Vector2 pointToCheck, Vector2 ellipsePos, Vector2 ellipseSize)
        {
            return isPointInsideEllipse(pointToCheck, ellipsePos, ellipseSize.x, ellipseSize.y);
        }
        public static bool isPointInsideEllipse(Vector2 pointToCheck, Vector2 ellipsePos, float ellipseWidth, float ellipseHeight)
        {
            if (ellipseWidth <= 0 || ellipseHeight <= 0)
            {
                return false;
            }
            float dx = pointToCheck.x - ellipsePos.x;
            float dy = pointToCheck.y - ellipsePos.y;
            float xComponent = (dx * dx / (ellipseWidth * ellipseWidth));
            float yComponent = (dy * dy / (ellipseHeight * ellipseHeight));
            float value = xComponent + yComponent;

            if (value < 1)
            {
                return true;
            }
            return false;
        }
        public static float abs(float pValue)
        {
            return Mathf.Abs(pValue);
        }
        public static long abs(long pValue)
        {
            return pValue > 0 ? pValue : -pValue;
        }
        public static int abs(int pValue)
        {
            return Mathf.Abs(pValue);
        }
        public static bool approximately(float pValue, float pRefValue)
        {
            return Mathf.Approximately(pValue, pRefValue);
        }
        public static bool approximately(Vector3 pValue, Vector3 pRefValue)
        {
            bool ax = approximately(pValue.x, pRefValue.x);
            bool ay = approximately(pValue.y, pRefValue.y);
            bool az = approximately(pValue.z, pRefValue.z);
            return ax && ay && az;
        }
        public static float round(float pValue, int pDecimal)
        {
            return (float)System.Math.Round(pValue, pDecimal);
        }
        public static Vector3 round(ref Vector3 pVector, int pDecimal)
        {
            pVector.Set(round(pVector.x, pDecimal), round(pVector.y, pDecimal), round(pVector.z, pDecimal));
            return pVector;
        }
        public static Vector2 round(ref Vector2 pVector, int pDecimal)
        {
            pVector.Set(round(pVector.x, pDecimal), round(pVector.y, pDecimal));
            return pVector;
        }
        public static bool checkLineIntersect(Vector2 pLineStart, Vector2 pLineEnd, Rect pRect)
        {
            // check if the line has hit any of the rectangle's sides
            float x1 = pLineStart.x;
            float y1 = pLineStart.y;
            float x2 = pLineEnd.x;
            float y2 = pLineEnd.y;
            float rx = pRect.x;
            float ry = pRect.y;
            float rw = pRect.width;
            float rh = pRect.height;
            // uses the Line/Line function below
            bool left = lineLine(x1, y1, x2, y2, rx, ry, rx, ry + rh);
            bool right = lineLine(x1, y1, x2, y2, rx + rw, ry, rx + rw, ry + rh);
            bool top = lineLine(x1, y1, x2, y2, rx, ry, rx + rw, ry);
            bool bottom = lineLine(x1, y1, x2, y2, rx, ry + rh, rx + rw, ry + rh);

            // if ANY of the above are true, the line
            // has hit the rectangle
            if (left || right || top || bottom)
            {
                return true;
            }
            return false;
        }
        public static bool lineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            // calculate the direction of the lines
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                return true;
            }
            return false;
        }
        public static double pow(double f, double p)
        {
            return System.Math.Pow(f, p);
        }
        public static float pow(float f, float p)
        {
            return Mathf.Pow(f, p);
        }
        public static float sqrt(float f)
        {
            return Mathf.Sqrt(f);
        }
        public static int floorToInt(float f)
        {
            return Mathf.FloorToInt(f);
        }
        public static int ceilToInt(float f)
        {
            return Mathf.CeilToInt(f);
        }
        public const int Intersection_OUT_SIDE = 0;
        public const int Intersection_IN_SIDE = 1;
        public const int Intersection_CUT = 2;
        public static int intersectEllipseLine(Vector2 center, float rx, float ry, Vector2 a1, Vector2 a2)
        {
            int result = 0;
            var origin = a1;
            var dir = a2 - a1;
            var diff = origin - center;
            var mDir = new Vector2(dir.x / (rx * rx), dir.y / (ry * ry));
            var mDiff = new Vector2(diff.x / (rx * rx), diff.y / (ry * ry));

            var a = Vector2.Dot(dir, mDir);
            var b = Vector2.Dot(dir, mDiff);
            var c = Vector2.Dot(diff, mDiff) - 1.0f;
            var d = b * b - a * c;

            if (d < 0)
            {
                result = Intersection_OUT_SIDE;
            }
            else if (d > 0)
            {
                var root = sqrt(d);
                var t_a = (-b - root) / a;
                var t_b = (-b + root) / a;

                if ((t_a < 0 || 1 < t_a) && (t_b < 0 || 1 < t_b))
                {
                    if ((t_a < 0 && t_b < 0) || (t_a > 1 && t_b > 1))
                        result = Intersection_OUT_SIDE;
                    else
                        result = Intersection_IN_SIDE;
                }
                else
                {
                    result = Intersection_CUT;
                }
            }
            else
            {
                var t = -b / a;
                if (0 <= t && t <= 1)
                {
                    result = Intersection_CUT;
                }
                else
                {
                    result = Intersection_OUT_SIDE;
                }
            }

            return result;
        }
        public static bool intersectEllipseRectangle(Vector2 c, float rx, float ry, Rect pRect)
        {
            var tL = new Vector2(pRect.x, pRect.y + pRect.height);
            var tR = new Vector2(pRect.x + pRect.width, pRect.y + pRect.height);
            var bL = new Vector2(pRect.x, pRect.y);
            var bR = new Vector2(pRect.x + pRect.width, pRect.y);
            return intersectEllipseLine(c, rx, ry, tL, tR) == Intersection_CUT
                || intersectEllipseLine(c, rx, ry, tL, bL) == Intersection_CUT
                || intersectEllipseLine(c, rx, ry, bL, bR) == Intersection_CUT
                || intersectEllipseLine(c, rx, ry, bR, tR) == Intersection_CUT;
        }
        public static bool rectInsideEllipse(Vector2 c, float rx, float ry, Rect pRect)
        {
            var tL = new Vector2(pRect.x, pRect.y + pRect.height);
            var tR = new Vector2(pRect.x + pRect.width, pRect.y + pRect.height);
            var bL = new Vector2(pRect.x, pRect.y);
            var bR = new Vector2(pRect.x + pRect.width, pRect.y);
            return intersectEllipseLine(c, rx, ry, tL, tR) == Intersection_IN_SIDE
                && intersectEllipseLine(c, rx, ry, tL, bL) == Intersection_IN_SIDE
                && intersectEllipseLine(c, rx, ry, bL, bR) == Intersection_IN_SIDE
                && intersectEllipseLine(c, rx, ry, bR, tR) == Intersection_IN_SIDE;
        }
        private static bool inAngle(Vector2 v1, Vector2 v2, float pFromDeg, float pToDeg)
        {
            float deg = angleDeg(v1, v2);
            return (deg >= pFromDeg && deg <= pToDeg) || (deg + 360 >= pFromDeg && deg + 360 <= pToDeg);
        }
        public static bool intersectOrOverlapEllipseRectangle(Vector2 c, float rx, float ry, Rect pRect, float pFromAngle = 0, float pToAngle = 360, bool pRightSide = true, bool drawBox = false)
        {
            var tL = new Vector2(pRect.x, pRect.y + pRect.height);
            var tR = new Vector2(pRect.x + pRect.width, pRect.y + pRect.height);
            var bL = new Vector2(pRect.x, pRect.y);
            var bR = new Vector2(pRect.x + pRect.width, pRect.y);

            float a1 = pRightSide ? pFromAngle : pFromAngle + 180;
            float a2 = pRightSide ? pToAngle : pToAngle + 180;

            pFromAngle = a1;
            pToAngle = a2;

            var result = true;
            bool inAngleRange = true;
            if (pToAngle - pFromAngle != 360)
            {
                // check if any angle from center to rect point in angle range
                inAngleRange = (pRightSide ? pRect.xMax > c.x : pRect.xMin < c.x) &&
                    (inAngle(c, tL, pFromAngle, pToAngle)
                    || inAngle(c, tR, pFromAngle, pToAngle)
                    || inAngle(c, bL, pFromAngle, pToAngle)
                    || inAngle(c, bR, pFromAngle, pToAngle));
                if (inAngleRange == false)
                {
                    inAngleRange = false;
                    result = inAngleRange;
                }
            }
            if (inAngleRange)
            {
                var r1 = intersectEllipseLine(c, rx, ry, tL, tR);
                if (r1 != Intersection_CUT)
                {
                    var r2 = intersectEllipseLine(c, rx, ry, tL, bL);
                    if (r2 != Intersection_CUT)
                    {
                        var r3 = intersectEllipseLine(c, rx, ry, bL, bR);
                        if (r3 != Intersection_CUT)
                        {
                            var r4 = intersectEllipseLine(c, rx, ry, bR, tR);
                            if (r4 != Intersection_CUT)
                            {
                                result = r1 == Intersection_IN_SIDE
                                    || r2 == Intersection_IN_SIDE
                                    || r3 == Intersection_IN_SIDE
                                    || r4 == Intersection_IN_SIDE;
                            }
                        }
                    }
                }
            }
#if UNITY_EDITOR
            if (drawBox)
            {
                var resultColor = result ? Color.green : Color.red;
                if (inAngleRange == false)
                {
                    resultColor = Color.yellow;
                    // RLog.d("angle {0} {1} {2} {3}", angleDeg(c, tL), angleDeg(c, tR), angleDeg(c, bL), angleDeg(c, bR));
                }
                //*
                float x = c.x + rx * MathUtils.cosDeg(a1);
                float y = c.y + ry * MathUtils.sinDeg(a1);

                x = c.x + rx * MathUtils.cosDeg(a2);
                y = c.y + ry * MathUtils.sinDeg(a2);

                //*/
            }
#endif
            return result;
        }


        public static float Sum(float[] array)
        {
            float sum = 0;
            for (int i = 0; i < array.Length; i++)
                sum += array[i];
            return sum;
        }
    }
}
