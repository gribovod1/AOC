using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStruct
{
    public class BinaryTree<T>
    {
        BinaryTree<T> parent = null;
        public BinaryTree<T> Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        public int Level
        {
            get
            {
                return (Parent != null ? Parent.Level : 0) + 1;
            }
        }

        BinaryTree<T> left = null;
        public BinaryTree<T> Left
        {
            get
            {
                return left;
            }
            set
            {
                if (value != null)
                    value.Parent = this;
                left = value;
            }
        }

        BinaryTree<T> right = null;
        public BinaryTree<T> Right
        {
            get
            {
                return right;
            }
            set
            {
                if (value != null)
                    value.Parent = this;
                right = value;
            }
        }

        public bool IsLeaf
        {
            get
            {
                return Right == null && Left == null;
            }
        }

        T value;
        public T Value
        {
            get { return value; }
            set {  this.value = value; }
        }

        public BinaryTree()
        {
        }

        public BinaryTree(T value)
        {
            this.Value = value;
        }

        public BinaryTree(BinaryTree<T> parent)
        {
            this.parent = parent;
        }

        public BinaryTree(T value, BinaryTree<T> parent)
        {
            this.value = value;
            this.parent = parent;
        }

        public BinaryTree(T left, T right)
        {
            this.left = new BinaryTree<T>(left, this);
            this.right = new BinaryTree<T>(right, this);
        }

        public BinaryTree(BinaryTree<T> left, BinaryTree<T> right)
        {
            this.Left = left;
            this.Right = right;
        }

        public enum Direction
        {
            ToLeft,
            ToRight
        }

        public BinaryTree<T> GetNextLeaf(Func<BinaryTree<T>, bool> comparer, Direction direction, BinaryTree<T> relation = null)
        {
            if (relation == null)
            {
                if (Parent != null)
                {
                    return Parent.GetNextLeaf(comparer, direction, this);
                }
            }
            else
            {
                if (Left == relation && direction == Direction.ToRight)
                {
                    if (comparer(this))
                    {
                        return this;
                    }
                    if (Right != null)
                    {
                        var r = Right.FindFirst(comparer, direction);
                        if (r != null)
                        {
                            return r;
                        }
                    }
                }
                if (Right == relation && direction == Direction.ToLeft)
                {
                    if (comparer(this))
                    {
                        return this;
                    }
                    if (Left != null)
                    {
                        var l = Left.FindFirst(comparer, direction);
                        if (l != null)
                        {
                            return l;
                        }
                    }
                }
                if (Parent != null)
                {
                    return Parent.GetNextLeaf(comparer, direction, this);
                }
            }

            return null;
        }

        public BinaryTree<T> FindFirst(Func<BinaryTree<T>, bool> comparer, Direction direction)
        {
            BinaryTree<T> result = null;
            BinaryTree<T>[] trees = direction == Direction.ToRight
                ? new BinaryTree<T>[2] { Left, Right }
                : new BinaryTree<T>[2] { Right, Left };

            if (trees[0] != null)
            {
                result = trees[0].FindFirst(comparer, direction);
                if (result != null)
                    return result;
            }
            if (comparer(this))
                return this;
            if (trees[1] != null)
            {
                result = trees[1].FindFirst(comparer, direction);
                if (result != null)
                    return result;
            }
            return null;
        }

        public void Map(Action<BinaryTree<T>> action, Direction direction = Direction.ToRight)
        {
            if (direction == Direction.ToLeft)
            {
                if (Right != null)
                    action(Right);
                if (Left != null)
                    action(Left);
            }
            else
            {
                if (Left != null)
                    action(Left);
                if (Right != null)
                    action(Right);
            }
        }

        public void MapFrom(Action<BinaryTree<T>> action, Direction direction = Direction.ToRight)
        {
            if (direction == Direction.ToLeft)
            {
                if (Right != null)
                    action(Right);
                if (Left != null)
                    action(Left);
            }
            else
            {
                if (Left != null)
                    action(Left);
                if (Right != null)
                    action(Right);
            }
        }

        public bool Remove(BinaryTree<T> child)
        {
            if (Left == child)
            {
                Left = null;
                return true;
            }
            if (Right == child)
            {
                Right = null;
                return true;
            }
            return false;
        }

        public bool Remove()
        {
            if (Parent != null)
                return Parent.Remove(this);
            return false;
        }
    }
}