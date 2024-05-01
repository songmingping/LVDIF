using System.Collections.Generic;
using UnityEngine;

namespace NONE.LVDIF
{
    [System.Serializable]
    public class InputFrameData
    {
        public string TimeStamp;
        public int FrameID;
        public int InputMethod;
        public int BrushShape;
        public int InputRadius;
        public bool EraserMode;
        public int ColorType;
        public string StartTime;
        public string EndTime;
        public List<Vector4> CursorPositions;

        // 带参数的构造器
        public InputFrameData(
            string timeStamp,
            int frameID,
            int inputMethod,
            int brushShape,
            int inputRadius,
            bool eraserMode,
            int colorType,
            string startTime,
            string endTime,
            List<Vector4> cursorPositions)
        {
            TimeStamp = timeStamp;
            FrameID = frameID;
            InputMethod = inputMethod;
            BrushShape = brushShape;
            InputRadius = inputRadius;
            EraserMode = eraserMode;
            ColorType = colorType;
            StartTime = startTime;
            EndTime = endTime;
            CursorPositions = cursorPositions ?? new List<Vector4>();  
        }
    }

}
