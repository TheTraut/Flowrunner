namespace HW1
{
    public class Node(int value)
    {
        public int data = value;
        public Node? next = null;
    }

    public class HW1LinkedList
    {
        public Node? head;
        public Node? tail;

        public void CreateList(int[] elements)
        {
            if (elements.Length == 0)
            {
                head = null;
            }
            else
            {
                foreach (int element in elements)
                {
                    AddToTail(element);
                }
            }
        }

        public void AddToTail(int elementToAdd) {
            Node newNode = new Node(elementToAdd);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                tail.next = newNode;
            }

            tail = newNode;
        }

        public int Length() {

            int length = 0;
            Node node = head;

            while (node != null)
            {
                length++;
                node = node.next;
            }

            return length;
        }

        public bool IsEmpty() { 
            return head == null;
        }
    }
}