using UnityEditor;

class BuildScript
{
	static void PerformBuild ()
	{
		string[] scenes = {
			"Assets/GGJ/Scenes/TutorialSymbols.unity",
			"Assets/GGJ/Scenes/Main.unity",
			"Assets/GGJ/Scenes/ScoreScene.unity",
			"Assets/GGJ/Scenes/GameUI.unity",
			"Assets/GGJ/Scenes/VFX.unity"
		};

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = scenes;
		buildPlayerOptions.locationPathName = "WebGLBuild";
		buildPlayerOptions.target = BuildTarget.WebGL;
		buildPlayerOptions.options = BuildOptions.None;
		BuildPipeline.BuildPlayer(buildPlayerOptions);
	}
}
