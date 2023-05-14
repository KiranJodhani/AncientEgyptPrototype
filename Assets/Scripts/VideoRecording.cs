
#if UNITY_EDITOR

using System.ComponentModel;
using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;
using TMPro;
public class VideoRecording : MonoBehaviour
{
    RecorderController m_RecorderController;
    public bool m_RecordAudio = true;
    internal MovieRecorderSettings m_Settings = null;

    public TextMeshProUGUI RecordingText;
    public FileInfo OutputFile
    {
        get
        {
            string TimeStamp= System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            var fileName = m_Settings.OutputFile+ TimeStamp + ".mp4";
            return new FileInfo(fileName);
        }
    }

    void Awake()
    {
        Initialize();
        
    }

    internal void Initialize()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        var mediaOutputFolder = new DirectoryInfo(Path.Combine(Application.dataPath, "..", "SampleRecordings"));

        // Video
        m_Settings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        m_Settings.name = "GamePlay";
        m_Settings.Enabled = true;

        // This example performs an MP4 recording
        m_Settings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        m_Settings.VideoBitRateMode = VideoBitrateMode.High;

        m_Settings.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = Screen.width,
            OutputHeight = Screen.height
        };

        m_Settings.AudioInputSettings.PreserveAudio = m_RecordAudio;

        // Simple file name (no wildcards) so that FileInfo constructor works in OutputFile getter.
        m_Settings.OutputFile = mediaOutputFolder.FullName + "/" + "video";

        // Setup Recording
        controllerSettings.AddRecorderSettings(m_Settings);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;

        RecorderOptions.VerboseMode = false;
        m_RecorderController.PrepareRecording();

        
    }

    public void ToggleRecording()
    {
        if(RecordingText.text== "Stop Recording")
        {
            m_RecorderController.StopRecording();
            RecordingText.text = "Start Recording";
        }
        else
        {
            m_RecorderController.StartRecording();
            Debug.Log($"Started recording for file {OutputFile.FullName}");
            RecordingText.text = "Stop Recording";
        }
    }
   
}

#endif