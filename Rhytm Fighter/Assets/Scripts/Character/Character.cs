using FrameworkPackage.PathCreation;
using Frameworks.Grid.View;
using PathCreation;
using RhytmFighter.Interfaces;
using UnityEngine;

namespace RhytmFighter.Characters
{
    /// <summary>
    /// Character wrapper
    /// </summary>
    public class Character : MonoBehaviour, iUpdateable
    {
        public event System.Action<int> OnMovementFinished;
        public event System.Action<int> OnCellVisited;

        public bool IsMoving => m_MovePathController.IsMoving;

        //Movement
        private float m_MoveSpeed;
        private PathCreator m_PathVisualizer;
        private MovePathController m_MovePathController;

        //Update position
        private int m_CurPathIndex;
        private float m_CurDistToUpdate;
        private float m_CellPositionUpdateDist;
        private float m_PassedDistanceSinceLastPoint;
        

        public void Initialize(Vector3 pos, float moveSpeed)
        {
            transform.position = pos;
            m_MoveSpeed = moveSpeed;

            //Create path controller
            m_MovePathController = new MovePathController(transform);
            m_MovePathController.OnMovementFinished += () =>
            {
                Debug.Log("MOVEMENT FINISHED");
                MonoBehaviour.Destroy(m_PathVisualizer);
                OnMovementFinished?.Invoke(++m_CurPathIndex);
            };
        }

        public void StartMove(Vector3[] path)
        {
            if (path != null)
            {
                //Заменить начальную точку движения с позиции клетки на позициию персонажа
                path[0] = m_MovePathController.ControlledTransform.position;

                //Создать кривую по массиву точек
                BezierPath bezierPath = GenerateBezierPath(path);
                VertexPath vertexPath = GenerateVertexPath(path);

                //Данные для обновления позиции при смене ячеек
                m_CellPositionUpdateDist = vertexPath.length / (path.Length - 1);
                m_CurDistToUpdate = m_CellPositionUpdateDist;
                m_PassedDistanceSinceLastPoint = 0;

                //Индекс клетки из массива пути, в котором сейчас пребывает персонаж
                m_CurPathIndex = 0;

                //Отображение пути
                m_PathVisualizer = m_MovePathController.ControlledTransform.gameObject.AddComponent<PathCreator>();
                m_PathVisualizer.bezierPath = bezierPath;

                //Начать движение
                m_MovePathController.StartMovement(vertexPath, m_MoveSpeed);
            }
        }

        public void StopMove() => m_MovePathController.StopMovement();

        public void PerformUpdate(float deltaTime)
        {
            if (IsMoving)
            {
                m_MovePathController.Update(deltaTime);

                //Обновление позиции ячейки
                float distTravelled = m_MovePathController.DistanceTravelled - m_PassedDistanceSinceLastPoint;
                if (distTravelled >= m_CurDistToUpdate)
                {
                    //Каждый раз при прохождении необходимой для обновления дистанции "расстояние с последнего обновления" задаеться текущему пройденному расстоянию.
                    m_PassedDistanceSinceLastPoint = m_MovePathController.DistanceTravelled;

                    //Текущее расстояние для обновления всегда после первого обновления изменяется на значение по-умолчанию (первое значение всегда меньше) 
                    m_CurDistToUpdate = m_CellPositionUpdateDist;

                    //Событие посещения ячейки (Индекс клетки из массива пути, в котором сейчас пребывает персонаж)
                    OnCellVisited?.Invoke(++m_CurPathIndex);
                }
            }
        }


        BezierPath GenerateBezierPath(Vector3[] points) => new BezierPath(points, false, PathSpace.xyz);

        VertexPath GenerateVertexPath(Vector3[] points)
        {
            // Create a closed, 2D bezier path from the supplied points array
            // These points are treated as anchors, which the path will pass through
            // The control points for the path will be generated automatically
            //BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
            // Then create a vertex path from the bezier path, to be used for movement etc
            return new VertexPath(GenerateBezierPath(points));
        }
    }
}
