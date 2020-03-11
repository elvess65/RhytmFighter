﻿using FrameworkPackage.PathCreation;
using PathCreation;
using System;
using UnityEngine;

namespace RhytmFighter.Characters.Movement
{
    public class Bezier_MovementStrategy : iMovementStrategy
    {
        public event Action OnMovementFinished;
        public event Action<int> OnCellVisited;
        public event Action OnMovementInterrupted;

        //Movement
        private float m_MoveSpeed;
        private PathCreator m_PathVisualizer;
        private MovePathController m_MovePathController;

        //Update position
        private int m_CurPathIndex;                         //Инекс клетки пути в которой находится персонаж
        private float m_CellPositionUpdateDist;             //Дистанция, которую нужно пройти для обновления позиции ячейки
        private float m_PassedDistanceSinceLastPoint;       //Дистанция, которую прошел персонаж с последнего обновления ячейки

        public bool IsMoving => m_MovePathController.IsMoving;


        public Bezier_MovementStrategy(Transform controlledTransform, float moveSpeed)
        {
            m_MoveSpeed = moveSpeed;

            //Create path controller
            m_MovePathController = new MovePathController(controlledTransform);
            m_MovePathController.OnMovementFinished += MovementFinishedHandler;

            //Create path visualizer
            m_PathVisualizer = m_MovePathController.ControlledTransform.gameObject.AddComponent<PathCreator>();
        }

        public void StartMove(Vector3[] path)
        {
            if (path != null)
            {
                //Заменить начальную точку движения с позиции клетки на позициию персонажа
                path[0] = m_MovePathController.ControlledTransform.position;

                //Создать кривую по массиву точек
                BezierPath bezierPath = null;                                       //Отображение
                VertexPath vertexPath = GenerateVertexPath(path, out bezierPath);   //Передвижение

                //Данные для обновления позиции при смене ячеек
                m_CellPositionUpdateDist = vertexPath.length / (path.Length - 1);
                m_PassedDistanceSinceLastPoint = 0;

                //Индекс клетки из массива пути, в котором сейчас пребывает персонаж
                m_CurPathIndex = 0;

                //Отображение пути
                m_PathVisualizer.enabled = true;
                m_PathVisualizer.bezierPath = bezierPath;

                //Начать движение
                m_MovePathController.StartMovement(vertexPath, m_MoveSpeed);
            }
        }

        public void StopMove() => m_MovePathController.StopMovement();

        public void Update(float deltaTime)
        {
            if (IsMoving)
            {
                //Перемещение по пути
                m_MovePathController.Update(deltaTime);

                //Обновление позиции ячейки
                float distTravelled = m_MovePathController.DistanceTravelled - m_PassedDistanceSinceLastPoint;

                //IsMoving нужен для случая, когда движение закончилось - тогда не нужно вызывать CellVisited
                if (IsMoving && distTravelled >= m_CellPositionUpdateDist)
                    CellVisitedHandler();
            }
        }


        void MovementFinishedHandler()
        {
            m_PathVisualizer.enabled = false;
            OnMovementFinished?.Invoke();
        }

        void CellVisitedHandler()
        {
            //Каждый раз при прохождении необходимой для обновления дистанции "расстояние с последнего обновления" задаеться текущему пройденному расстоянию.
            m_PassedDistanceSinceLastPoint = m_MovePathController.DistanceTravelled;

            //Событие посещения ячейки (Индекс клетки из массива пути, в котором сейчас пребывает персонаж)
            OnCellVisited?.Invoke(++m_CurPathIndex);
        }

        void MovementInterruptedHandler() => OnMovementInterrupted?.Invoke();


        VertexPath GenerateVertexPath(Vector3[] points, out BezierPath bezierPath)
        {
            // Create a closed, 2D bezier path from the supplied points array
            // These points are treated as anchors, which the path will pass through
            // The control points for the path will be generated automatically
            //BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
            // Then create a vertex path from the bezier path, to be used for movement etc
            bezierPath = GenerateBezierPath(points);
            bezierPath.GlobalNormalsAngle = 90;

            return new VertexPath(bezierPath);
        }

        BezierPath GenerateBezierPath(Vector3[] points) => new BezierPath(points, false, PathSpace.xyz);
    }
}