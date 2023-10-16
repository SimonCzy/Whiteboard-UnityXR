# Whiteboard-UnityXR

This README was updated on October 15th, 2023.

## Package

In this project, you need to import 4 package into your project. Even though these files are already included in this repository, I will still briefly introduce them here and attach the download link.

- [Oculus Integration](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022): You need to add into you account first and then import in Unity > Window > Package Manager > My Assets.
- [Meta Avatar SDK](https://developer.oculus.com/downloads/package/meta-avatars-sdk/): You need to download the file first and then import in Unity > Assers > Custom Package > Custom Package...
- [Photon Fusion](https://doc.photonengine.com/fusion/current/getting-started/sdk-download): You need to follow the instruction of installation. Remember to satisfy the requirements firstly.
- [Photon Voice 2](https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518): You need to add into you account first and then import in Unity > Window > Package Manager > My Assets.

## Learning Resource

- Youtuber: https://www.youtube.com/@JustinPBarnett
- Youtuber: https://www.youtube.com/@ValemVR
- Youtuber: https://www.youtube.com/@dilmerv
- Fusion Documentation: https://doc.photonengine.com/fusion/current/getting-started/fusion-intro
- Interation Example: https://developer.oculus.com/documentation/unity/unity-isdk-example-scenes/

## User Manual

![](https://scontent.fmel3-1.fna.fbcdn.net/v/t39.2365-6/64515613_622041711631269_1500694343822868480_n.png?_nc_cat=103&ccb=1-7&_nc_sid=e280be&_nc_ohc=7WtZrfzz4d8AX-DAu2-&_nc_ht=scontent.fmel3-1.fna&oh=00_AfCCuxKa6faBFtG-607N68NYf8l-zs9JtIJQ9o7AdZi38w&oe=65479C81)

### Create Whiteboard

Firstly, press SecondaryIndexTrigger to draw the horizontal line.

Then, press SecondaryIndexTrigger again to pull out a whiteboard from the line.

### Reset Whiteboard

Press Button.Four

### Call out debug display

Press Button.Four and PrimaryIndexTrigger at the same time.

**Note**: This action will reset whiteboard. So please press it before you create the whiteboard.

### Poke Interaction

Controllers: There is a white dot indicator shown in the front of the controller.

Hands: Use index finger as indicator.

### Grab Interaction

Controllers: Use PrimaryHandTrigger or SecondaryHandTrigger.

Hands: Bring the thumb and index finger together.