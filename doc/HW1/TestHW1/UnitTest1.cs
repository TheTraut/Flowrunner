using HW1;

namespace TestHW1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreateList_WithElements()
        {
            HW1LinkedList testList = new();

            int[] intList = [1, 2, 3, 4, 5];
            testList.CreateList(intList);

            Node currentNode = testList.head;
            int index = 0;
            while (currentNode != null)
            {
                Assert.That(currentNode.data, Is.EqualTo(intList[index]));
                currentNode = currentNode.next;
                index++;
            }
            Assert.That(testList.head, Is.Not.Null);
        }

        [Test]
        public void TestCreateList_NoElements()
        {
            int[] intList = [];
            HW1LinkedList testList = new();
            testList.CreateList(intList);
            Assert.That(testList.head, Is.Null);
        }

        [Test]
        public void TestLength_MatchInputLength()
        {
            HW1LinkedList testList = new();

            int[] intList = [1, 2, 3, 4, 5];
            testList.CreateList(intList);

            Assert.That(testList.Length(), Is.EqualTo(intList.Length));
        }

        [Test]
        public void TestIsEmpty_True()
        {
            HW1LinkedList testList = new();

            int[] intList = [];
            testList.CreateList(intList);

            Assert.That(testList.IsEmpty(), Is.True);
        }

        [Test]
        public void TestIsEmpty_False()
        {
            HW1LinkedList testList = new();

            int[] intList = [1, 2, 3, 4, 5];
            testList.CreateList(intList);

            Assert.That(testList.IsEmpty(), Is.False);
        }

        [Test]
        public void TestAddToTail_AddsValueToEndOfList()
        {
            HW1LinkedList testList = new();

            int[] intList = [];
            testList.CreateList(intList);

            int toBeAdded = 6;

            testList.AddToTail(toBeAdded);

            Node currentNode = testList.head;
            while (currentNode.next != null)
            {
                currentNode = currentNode.next;
            }
            Assert.AreEqual(toBeAdded, currentNode.data);
        }
    }
}