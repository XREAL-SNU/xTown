using UnityEngine;
using static System.Linq.Enumerable;

namespace XReal.XTown.Basketball
{
    public class SevenSegmentDisplay : MonoBehaviour
    {
        public Material onMaterial;
        public Material offMaterial;
        private Transform[] segments;
        private const int NumSegments = 7;

        private class Shape
        {
            private readonly byte shapeBits;

            public Shape(params byte[] segmentIndexes)
            {
                shapeBits = (byte)segmentIndexes.Aggregate(0, (segmentBits, segmentIndex) =>
                   segmentBits | (1 << segmentIndex));
            }

            public bool HasSegment(int segmentIndex)
            {
                int bit = 1 << segmentIndex;
                return (shapeBits & bit) == bit;
            }
        }

        private static readonly Shape[] digitShapes = {
        new Shape(0,    2, 3, 4, 5, 6),
        new Shape(            4,    6),
        new Shape(0, 1, 2,    4, 5   ),    //   |    2    | 
        new Shape(0, 1, 2,    4,    6),    //   |3  ---  4| 
        new Shape(   1,    3, 4,    6),    //   |    1    | 
        new Shape(0, 1, 2, 3,       6),    //   |5  ---  6| 
        new Shape(0, 1, 2, 3,    5, 6),    //   |    0    | 
        new Shape(      2,    4,    6),    //       ---     
        new Shape(0, 1, 2, 3, 4, 5, 6),
        new Shape(0, 1, 2, 3, 4, 6   )
    };

        private int digitShowing;

        public void SetDigit(int digit)
        {
            digitShowing = digit;
            LightSegments();
        }

        private Renderer[] renderers;

        private void Awake()
        {
            renderers = transform.Find("Segments").GetComponentsInChildren<Renderer>();
            SetDigit(0);
        }

        private void LightSegments()
        {
            var shape = digitShapes[digitShowing];
            for (int i = 0; i < NumSegments; i++)
            {
                var segmentRenderer = renderers[i];
                var on = shape.HasSegment(i);
                segmentRenderer.material = on ? onMaterial : offMaterial;
            }
        }

        public void TurnOff()
        {
            foreach (var r in renderers) r.material = offMaterial;
        }
    }
}
