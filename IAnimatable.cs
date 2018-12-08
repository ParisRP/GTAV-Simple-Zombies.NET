using GTA;

namespace ZombiesMod
{
  public interface IAnimatable
  {
    string AnimationDict { get; set; }

    string AnimationName { get; set; }

    AnimationFlags AnimationFlags { get; set; }

    int AnimationDuration { get; set; }
  }
}
