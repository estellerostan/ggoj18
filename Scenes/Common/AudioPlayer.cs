using Godot;

public class AudioPlayer: Spatial
{
    //AudioStream audioKnifeFire = (AudioStream)GD.Load("res://path_to_your_audio_here");
    AudioStream audioPistolShot = (AudioStream)GD.Load("res://Entities/Weapons/Pistol/Assets/Pistol_fire.wav");
    //udioStream audioRifleShot = (AudioStream)GD.Load("res://path_to_your_audio_here");

    AudioStreamPlayer audioNode = null;

    public override void _Ready()
    {
        audioNode = (AudioStreamPlayer)GetNode("AudioStreamPlayer");
        audioNode.Stop();
    }

    public void PlaySound(string soundName)
    {
        //if(audioKnifeFire == null || audioPistolShot == null || audioRifleShot == null)
        if(audioPistolShot == null)
        {
            GD.PrintErr("Audio not set!");
            QueueFree();
            return;
        }

        /*if(soundName == "Knife_fire")
        {
            audioNode.SetStream(audioKnifeFire);
        }
        else*/
        if(soundName == "Pistol_fire")
        {
            audioNode.SetStream(audioPistolShot);
        }
        /*else if(soundName == "Rifle_fire")
        {
            audioNode.SetStream(audioRifleShot);
        }*/
        else
        {
            GD.PrintErr("UNKNOWN STREAM");
            QueueFree();
            return;
        }

        audioNode.Play();
    }

    private void _OnAudioStreamPlayer_finished()
    {
        audioNode.Stop();
        QueueFree();
    }
}
