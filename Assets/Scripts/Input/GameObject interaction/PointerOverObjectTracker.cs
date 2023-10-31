using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using Zenject;

namespace LittleKingdom.Input
{
    public class PointerOverObjectTracker : IFixedTickable
    {
        public const int MaxObjects = 10;

        private readonly RaycastFromPointer raycastFromPointer;
        private readonly RaycastHit[] raycastHits = new RaycastHit[MaxObjects];
        private readonly GameObject[] currentHoveredObjects = new GameObject[MaxObjects];
        private readonly GameObject[] newHoveredObjects = new GameObject[MaxObjects];

        private Mode mode = Mode.TrackFirst;
        private GameObject newHoveredObject = null;

        /// <summary>
        /// The <see cref="GameObject"/> closest to the camera that has a collider and the pointer is currently hovering over.<br/>
        /// <see langword="null"/> if <see cref="Mode.TrackMany"/> is set.
        /// </summary>
        public GameObject HoveredObject { get; private set; }

        /// <summary>
        /// The <see cref="GameObject"/>s closest to the camera that have a collider and the pointer is currently hovering over,
        /// up to <see cref="MaxObjects"/>.<br/>
        /// Empty if <see cref="Mode.TrackFirst"/> is set.
        /// </summary>
        public ReadOnlyArray<GameObject> HoveredObjects { get; private set; }

        public event SimpleEventHandler<GameObject> ObjectEntered;
        public event SimpleEventHandler<GameObject> ObjectExited;

        public enum Mode
        {
            /// <summary>
            /// Tracks only the top most object under the pointer.
            /// </summary>
            TrackFirst,

            /// <summary>
            /// Tracks all objects under the pointer up to the specified amount.
            /// Note that the order is not guaranteed nor is it guaranteed to return the closest <see cref="GameObjects"/>.
            /// </summary>
            TrackMany
        }

        public PointerOverObjectTracker(RaycastFromPointer raycastFromPointer)
        {
            this.raycastFromPointer = raycastFromPointer;

            HoveredObjects = new ReadOnlyArray<GameObject>(currentHoveredObjects);
        }

        /// <summary>
        /// Changing mode will clear all tracked objects. Meaning, a <see cref="GameObject"/>
        /// could be immediately tracked by the new mode and fire events for it.
        /// </summary>
        /// <param name="mode"></param>
        public void SetMode(Mode mode)
        {
            this.mode = mode;

            if (mode == Mode.TrackFirst)
                Array.Clear(currentHoveredObjects, 0, MaxObjects);
            else
                HoveredObject = null;
        }

        public void FixedTick()
        {
            if (mode == Mode.TrackFirst)
                TrackFirstObject();
            else
                TrackManyObjects();
        }

        private void TrackFirstObject()
        {
            GetObjectUnderPointer();
            InvokeSingleObjectEvents(HoveredObject, newHoveredObject);
            HoveredObject = newHoveredObject;
        }

        private void InvokeSingleObjectEvents(GameObject currentObjectUnderPointer, GameObject newObjectUnderPointer)
        {
            if (currentObjectUnderPointer == newObjectUnderPointer)
                return;

            if (currentObjectUnderPointer != null)
                ObjectExited?.Invoke(currentObjectUnderPointer);

            if (newObjectUnderPointer != null)
                ObjectEntered?.Invoke(newObjectUnderPointer);
        }

        private void TrackManyObjects()
        {
            GetObjectsUnderPointer();
            InvokeManyObjectEvents();

            for (int i = 0; i < MaxObjects; i++)
            {
                currentHoveredObjects[i] = newHoveredObjects[i];
            }
        }

        private void InvokeManyObjectEvents()
        {
            foreach (GameObject gameObject in newHoveredObjects)
            {
                if (gameObject && !currentHoveredObjects.Contains(gameObject))
                    ObjectEntered?.Invoke(gameObject);
            }

            foreach (GameObject gameObject in currentHoveredObjects)
            {
                if (gameObject && !newHoveredObjects.Contains(gameObject))
                    ObjectExited?.Invoke(gameObject);
            }
        }

        private void GetObjectsUnderPointer()
        {
            Array.Clear(newHoveredObjects, 0, MaxObjects);
            Array.Clear(raycastHits, 0, MaxObjects);

            raycastFromPointer.CastAllTo3D(raycastHits);

            for (int i = 0; i < raycastHits.Length; i++)
            {
                RaycastHit hit = raycastHits[i];
                if (hit.collider != null)
                    newHoveredObjects[i] = hit.collider.gameObject;
            }
        }

        private void GetObjectUnderPointer() =>
            newHoveredObject = raycastFromPointer.CastTo3D(out RaycastHit hit) ?
                                   hit.collider.gameObject : null;
    }
}