// MP1: XML Validator
// Implementation of a simple stack for XMLTags.

// You should implement this class.
// You should add comments and improve the documentation.

using System;
using System.Collections.Generic; 

// An obvious reminder: 
// You are only allowed to use List<T> to code your
// own implemenation of a stack type, called MyStack

namespace XMLValidatorNS
{
    public class MyStack
    {
        // A List to hold XMLTag objects.
        // Use this to implement the requested stack.
        private List<XMLTag> stackInternal;

        /// <summary>
        /// Create an empty stack.
        /// </summary>
        public MyStack()
        {
            this.stackInternal = new List<XMLTag>();
        }

        /// <summary>
        /// Push a tag onto the top of the stack.
        /// </summary>
        public void Push(XMLTag tag)
        {
             
        }

        /// <summary> 
        /// Removes the tag at the top of the stack.
        /// Should throw an exception if the stack is empty. 
        /// </summary>
        public XMLTag Pop()
        {
            return null; // return a dummy value for now: ToFix
        }


        /// <summary>
        /// Looks at the object at the top of the stack but does not actually remove the object. 
        /// Should throw an exception if the stack is empty.
        /// </summary>
        public XMLTag Peek()
        {
            return null; // return a dummy value for now: ToFix
        }

        /// <summary>
        /// Tests if the stack is empty.
        /// Returns true if the stack is empty; false otherwise.
        /// </summary>
        public bool IsEmpty()
        {
            return false; // return a dummy value for now: ToFix
        }
    }
}
