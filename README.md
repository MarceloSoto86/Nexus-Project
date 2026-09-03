# Nexus-Project
Nexus-Special-Project

# 🌆 Nexus: Cyberpunk Noir Platformer (MVP)

> A precision vertical platformer featuring an evolving UI mental state mechanic, responsive physics, and state-machine-driven player control built in Unity.

!Game Engine: [Unity]
!Programming Language: C#
!Build Platform: Windows
!Itch.io Link: https://siddhartha86.itch.io/nexus
---

## 🎮 Game Overview
**Nexus** is a 3D vertical platformer set in an opressive cyberpunk-noir universe. Players control the *Supervisor*, an operative navigating toxic flooded structures while suffering from severe amnesia and mental fragmentation. To reach the top of the facility and escape, players must manage their jump/dash mechanics while collecting memory serums that stabilize both their energy and their visual perception of identity.

* **Genre:** 3D Vertical Precision Platformer / Cyberpunk Noir
* **Target Platform:** PC (Keyboard)
* **Current Status:** Functional MVP (Minimum Viable Product) featuring complete game loop, persistence, and reactive UI system.
* **Role:** Solo Developer — Game Design, Programming, Level Design, UI, VFX, Audio & Technical Implementation.

---

## 🛠️ Key Technical Features & Architecture

### 1. State Machine Architecture (`PlayerController`)
Built using a clean **Finite State Machine (FSM)** pattern to handle state switching (Grounded, Airborne, Dashing, Dying) seamlessly without cluttering `Update()` loops.
* Implemented manual physics tweaks (custom gravity scales, velocity clamping) to deliver responsive platforming mechanics.
* Integrated a dynamic dash ability tied to UI cooldown feedback.

### 2. Event-Driven Dynamic UI System (`HUDMemoryFaceMgr`)
Rather than relying on continuous `Update()` polling, UI elements respond reactively to gameplay events using C# `delegates` and static events (`PlayerEvents`).
* **Data-Driven Progression:** Uses an array of `ScriptableObjects` (`MemoryFlashes`) pre-casted during initialization for zero-overhead runtime performance.
* **Evolving Avatar:** As the player collects memory items, the HUD portrait dynamically transitions from a 90% glitched/distorted state to a crisp, high-resolution portrait, visually representing restored identity.

### 3. Decoupled Audio Pipeline (`AudioMixer`)
* Configured Unity's `AudioMixer` with dedicated sub-busses (`BGM` and `SFX`).
* Features a logarithmic volume slider formula ($20 \times \log_{10}(v)$) in the settings menu to match human hearing perception and prevent audio masking.

### 4. Save & Load Persistence System (`SaveSystem`)
* Implemented JSON-based local serialization to store player position, unlocked abilities (dash status), current sanity levels, and collected item IDs (`uniqueID` check) to prevent item duplication upon re-entering levels.

---

## 🧠 Key Design Decisions

1. **Decoupled Architecture:** Chose a static Event Channel system (`PlayerEvents.RaiseMemoryStoreUnlocked()`) over direct component references. This ensured that pickup items (`MemorySerum`) could trigger UI updates and sound queues without holding hard references to managers, preventing null references during scene reloads.
2. **Visual Feedback Over Complex Systems:** Designed the portrait evolution mechanic as a core feedback loop. Giving immediate visual clarity to the player's HUD upon collecting serums provided a stronger narrative tie to the gameplay loop than traditional health bars.
3. **Data-Driven Performance:** Pre-casting ScriptableObject arrays at `Start()` avoided expensive dynamic casting during event triggers, keeping runtime memory usage predictable.

---

## 🔬 Retrospective & Lessons Learned (In Hindsight)

If I were to approach this project again or scale it into full production, I would alter the following approaches:

* **Coyote Time & Input Buffering:** While the physics-based jumping feels solid, adding a 0.1s *Coyote Time* window and *Input Buffering* for airborne inputs would drastically increase the forgiveness and responsiveness ("Game Feel") of precision platforming.
* **ScriptableObject Event Architecture:** Transitioning from static C# events to a full ScriptableObject-based event channel system would make UI and audio wiring even more modular within the Unity Inspector.
* **Asynchronous Scene Loading:** Implementing an `AsyncOperation` loading manager instead of direct scene switching to allow for smooth screen fades and asset pre-loading during transitions between the Main Menu and gameplay levels.

---

## 💻 Tech Stack
* **Game Engine:** Unity (C#)
* **UI/UX:** Unity UI, TextMeshPro, Custom Shaders/Sprites
* **Audio:** Unity AudioMixer
* **Version Control:** Git / GitHub
