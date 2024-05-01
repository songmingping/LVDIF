using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ChaosIkaros.LVDIF;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace NONE.LVDIF
{
    public class InputRecorderJSON : MonoBehaviour
    {
        public static string GetTime()
        {
            return System.DateTime.Now.ToString("yyyy_MM_dd-hh:mm:ss.ffffff");
        }
        public static string GetTimeForFileName()
        {
            return System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_ffffff");
        }
        public static InputRecorderJSON holder;
        private string dirpath;
        private string fileName;
        private List<FileInfo> fileList;
        private List<string> fileNameList;

        public GameObject allUI;
        public TMP_Dropdown recordingDropdown;
        public TMP_Text recordingButtonText;
        public Text debugLogger;
        public int maxLength = 1000;
        public int dataCounter = 0;
        public bool isRecording = false;
        public int frameIDCounter = 0;
        [Header("Data Preview")]
        public InputFrameDataWrapper inputWrapper = new();
        public InputFrameDataWrapper outputWrapper = new();
        void Start()
        {
            holder = this;
            dirpath = Application.dataPath + "/Large Volume Data Interaction Framework/Recordings/";
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            //outputData.Add(GetTime() + ",1,0,0,0,none,none,32-32-32-10_64-64-64-10\r\n");
            //OutputCSV(dirpath + "Recording_testing.csv", recordingTypes, outputData);
            FileProcessor();
        }

        public void StartRecording()
        {
            if (!isRecording)
            {
                isRecording = true;
                debugLogger.text = "Recording";
                recordingButtonText.text = "Stop Recording";
                frameIDCounter = 0;
                outputWrapper.InputFrameDatas.Clear();
            }
            else
                StopRecording();
        }

        private void StopRecording()
        {
            isRecording = false;
            debugLogger.text = "Stop Recording";
            recordingButtonText.text = "Start Recording";
            if (outputWrapper.InputFrameDatas.Count != 0)
            {
                string json = JsonUtility.ToJson(outputWrapper, true);

                string currentTime = GetTimeForFileName();
                string outputPath = dirpath + "Recording_" + CudaMarchingCubesChunks.cudaMCManager.gridSize.x + "_" + currentTime + ".json";

                try
                {
                    File.WriteAllText(outputPath, json);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to save data to {outputPath}: {ex.Message}");
                }


                //string path = dirpath + "SDF_" + CudaMarchingCubesChunks.cudaMCManager.gridSize.x + "_" + currentTime + ".bv";
                //CudaBridge.SaveSdfData(CudaMarchingCubesChunks.cudaMCManager.chunks[0].GetComponent<CudaMarchingCubesChunk>().chunkID, path);
            }
            FileProcessor();
        }

        public void RecordOneFrame()
        {
            if (isRecording)
            {   
                // 创建新的 InputFrameData 对象
        InputFrameData newFrame = new InputFrameData(
            GetTime(),
            frameIDCounter,
            (int)BrushManager.holder.inputMethod,
            (int)BrushManager.holder.brushShape,
            (int)Mathf.Clamp(BrushManager.holder.inputRadius, BrushManager.holder.minInputRadius, BrushManager.holder.maxInputRadius),
            BrushManager.holder.eraserMode,
            (int)BrushManager.holder.colorType,
            BrushManager.holder.brushStartTime,
            BrushManager.holder.brushEndTime,
            new List<Vector4>(BrushManager.holder.finalCursorPosInBrush) 
        );

        // 添加到 outputWrapper
        outputWrapper.InputFrameDatas.Add(newFrame);

        frameIDCounter++;
            }
        }

        public void FileProcessor()
        {
            DirectoryInfo folder = new DirectoryInfo(dirpath);
            FileInfo[] files = folder.GetFiles();
            fileList = new List<FileInfo>();
            fileNameList = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.StartsWith("R") && files[i].Name.Contains(".json") && !files[i].Name.Contains(".meta"))
                {
                    //if (files[i].Name.Split('_').Length == 2)
                    //{
                    //Debug.Log(files[i].Name);
                    fileList.Add(files[i]);
                    fileNameList.Add(files[i].Name);
                    //}
                }
            }
            recordingDropdown.ClearOptions();
            recordingDropdown.AddOptions(fileNameList);
        }

        public void LoadFileByDropdown()
        {
            LoadFile(recordingDropdown.value);
        }

        private void LoadFile(int fileID)
        {
            string fileName = fileList[fileID].Name;
            if (!File.Exists(dirpath + fileName))
            {
                debugLogger.text = "Cannot find the file:" + dirpath + fileName;
                return;
            }
            string text = File.ReadAllText(dirpath + fileName);
            inputWrapper = JsonUtility.FromJson<InputFrameDataWrapper>(text);

            if (inputWrapper == null || inputWrapper.InputFrameDatas == null || inputWrapper.InputFrameDatas.Count == 0)
            {
                Debug.LogError("Failed to parse data from JSON.");
            }
            else
            {
                foreach (var frame in inputWrapper.InputFrameDatas)
                {
                    Debug.Log($"Frame {frame.FrameID}: {frame.TimeStamp}");
                }
                dataCounter = inputWrapper.InputFrameDatas.Count;
            }

            //ExportAnalysisReport(fileName);
            StartCoroutine(RecoridngPlayer());
        }

        public void ExportAnalysisReport(string fileName)
        {
            // outputData.Clear();
            // float averageInputTime = 0;
            // float totalInputTime = 0;
            // int inputTimeCounter = 0;
            // float averageBrushDistance = 0;
            // float totalBrushDistance = 0;
            // for (int i = 0; i < inputFrames.Count; i++)
            // {
            //     if (inputFrames[i].inputMethod > 0)
            //     {
            //         inputTimeCounter++;
            //         totalInputTime += (float)(inputFrames[i].endTime - inputFrames[i].startTime).TotalSeconds;
            //         for (int j = 1; j < inputFrames[i].cursorVoxelPosList.Count; j++)
            //         {
            //             Vector4 tempVector = inputFrames[i].cursorVoxelPosList[j] - inputFrames[i].cursorVoxelPosList[j - 1];
            //             totalBrushDistance += (new Vector3(tempVector.x, tempVector.y, tempVector.z)).magnitude;
            //         }
            //     }
            // }
            // averageInputTime = totalInputTime / inputTimeCounter;
            // averageBrushDistance = totalBrushDistance / inputTimeCounter;

            // outputData.Add("0" + "," + totalInputTime + "," + totalBrushDistance + "," +
            //     averageInputTime + "," + averageBrushDistance + "\r\n");
            // OutputCSV(dirpath + "Analysis_" + fileName, analysisTypes, outputData);
        }

        public IEnumerator RecoridngPlayer()
        {
            yield return new WaitForSeconds(1);
            debugLogger.text = "Playing recording frame: count " + inputWrapper.InputFrameDatas.Count;
            if (CudaMarchingCubesChunks.cudaMCManagerHolder.chunks.Count == 0)
            {
                debugLogger.text = "Playing recording frame: has not active chunk";
                yield break;
            }
            if (inputWrapper.InputFrameDatas.Count == 0)
            {
                debugLogger.text = "Playing recording frame: has not input frame";
                yield break;
            }
            BrushManager.currentChunk = CudaMarchingCubesChunks.cudaMCManagerHolder.chunks[0].GetComponent<CudaMarchingCubesChunk>();
            BrushManager.holder.cursor.GetComponent<MeshRenderer>().enabled = false;
            for (int i = 0; i < inputWrapper.InputFrameDatas.Count; i++)
            {
                debugLogger.text = "Playing recording frame: " + i;
                BrushManager.holder.eraserMode = inputWrapper.InputFrameDatas[i].EraserMode;
                BrushManager.holder.SetInputMethod(inputWrapper.InputFrameDatas[i].InputMethod);
                BrushManager.holder.SetBrushShape(inputWrapper.InputFrameDatas[i].BrushShape);
                BrushManager.holder.inputRadius = Mathf.Clamp(inputWrapper.InputFrameDatas[i].InputRadius,
                    BrushManager.holder.minInputRadius, BrushManager.holder.maxInputRadius);
                BrushManager.holder.colorType = inputWrapper.InputFrameDatas[i].ColorType;
                BrushManager.holder.UpdateColor();
                BrushManager.holder.finalCursorPosInBrush.Clear();
                BrushManager.holder.finalCursorPosInBrush.AddRange(
                    inputWrapper.InputFrameDatas[i].CursorPositions
                    );
                BrushManager.currentChunk.hasInput = true;
                CudaMarchingCubesChunks.cudaMCManager.stop = false;
                yield return new WaitUntil(() => !BrushManager.currentChunk.hasInput);
            }

            BrushManager.holder.SetBrushShape((int)BrushShape.Sphere);
            BrushManager.holder.SetInputMethod((int)InputMethod.SingleBrush);
            BrushManager.holder.eraserMode = false;
            BrushManager.holder.colorType = 0;
            BrushManager.holder.cursor.GetComponent<MeshRenderer>().enabled = true;
        }

        // public void OutputCSV(string fileName, string types, List<string> allData)
        // {
        //     if (!File.Exists(fileName))
        //         File.Create(fileName).Dispose();
        //     Stream stream = File.OpenWrite(fileName);
        //     BufferedStream bfs = new BufferedStream(stream);
        //     bfs.Seek(0, SeekOrigin.Begin);
        //     bfs.SetLength(0);//clear file
        //     byte[] buffType = new UTF8Encoding().GetBytes(types);
        //     bfs.Write(buffType, 0, buffType.Length);
        //     for (int i = 0; i < allData.Count; i++)
        //     {
        //         byte[] buffData = new UTF8Encoding().GetBytes(allData[i]);
        //         bfs.Write(buffData, 0, buffData.Length);
        //     }
        //     bfs.Flush();
        //     bfs.Close();
        //     stream.Close();
        //     Debug.Log("Saved file: " + fileName);
        // }
    }
}
