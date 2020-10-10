// MP1: XML Validator
// You should not modify this file!
// You should read the code here to understand how the XMLTag class is implemented.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XMLValidatorNS
{
    public class XMLTag
    {
        private readonly string Element;
        private readonly bool IsOpenTag;
        private readonly bool IsSelfCLosing;

        /// <summary>
        /// Constructs an XML "opening" tag with the given element (e.g. "summary").
        /// Throws a NullReferenceException if element is null. 
        /// effects: creates a new XMLTag object for this tag
        /// </summary>
        /// <param name="element">element that represents an opening non-selfclsoing 
        /// XML tag such as &lt;summary&gt; 
        /// </param>
        public XMLTag(String element) : this(element, true, false) { }

        /// <summary>
        /// Constructs an XML tag with the given element (e.g. "summary") and type.
        /// Self-closing tags like &lt; para /&gt; are considered to be also "opening" 
        /// tags.
        /// effects: creates an XMLTag object for the provided tag
        /// throws a NullPointerException if element is null. 
        /// </summary>
        /// <param name="element">element represents an XML tag such as &lt;p&gt; or&lt;/p&gt;
        /// </param>
        /// <param name="isOpenTag">isOpenTag indicates if the tag in an opening tag
        /// (true) or closing tag(false)</param>
        /// <param name="IsSelfCLosing">IsSelfCLosing indicates if the tag in a 
        /// self-closing tag</param>
        public XMLTag(String element, bool isOpenTag, bool IsSelfCLosing)
        {
            this.Element = element;  //xml elements are case-sensitive
            this.IsOpenTag = isOpenTag;
            this.IsSelfCLosing = IsSelfCLosing;
        }

        /// <summary>
        /// Returns true if this tag has the same element and type as the given other
        /// tag. 
        /// </summary>
        /// <param name="o">o is the object to compare this XMLTag to</param>
        /// <returns>true if this XMLTag is equal to o and false otherwise</returns>
        public bool IsEqual(Object o)
        {
            if (o is XMLTag other)
            {
                return Element.Equals(other.Element)
                         && IsOpenTag == other.IsOpenTag
                         && IsSelfCLosing == other.IsSelfCLosing;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns this XML tag's element, such as "summary" or "param". 
        /// </summary>
        /// <returns>a string representation of the tag represented by this XMLTag 
        /// without the &lt;&gt; brackets</returns>
        public string GetElement()
        {
            return Element;
        }

        /// <summary>
        /// Returns true if this XML tag is an "opening" (starting) tag and false
        /// if it is a closing tag.
        /// Self-closing tags like &lt;para /&gt; are considered to also be "opening" tags.
        /// </summary>
        /// <returns>true if this XMLTag is an opening tag or a self-closing tag,
        /// and false otherwise</returns>
        public bool GetIsOpenTag()
        {
            return IsOpenTag;
        }

        /// <summary>
        /// Returns true if this tag is a self-closing tag (e.g., &lt;see /&gt;).
        /// </summary>
        /// <returns>true if this XMLTag is self-closing and false otherwise</returns>
        public bool IsSelfClosing()
        {
            return IsSelfCLosing;
        }

        /// <summary>
        /// A match occurs when one tage is an opening tag and the other is a closing
        /// tag.
        /// Returns true if the given other tag is non-null and matches this tag;
        /// that is, if they have the same element but opposite types,
        /// such as &lt;body&gt; and &lt;/body&gt;.
        /// </summary>
        /// <param name="other">other is an XMLTag to compare this XMLTag with</param>
        /// <returns>true if this XMLTag "matches" the other XMLTag and false 
        /// otherwise.</returns>
        public bool Matches(XMLTag other)
        {
            return other != null
                && string.Equals(Element, other.Element, StringComparison.Ordinal) //case sensitive
                && IsOpenTag != other.IsOpenTag;
        }

        /// <summary>
        /// Returns a string representation of this XML tag, such as "&lt;/summary&gt;".
        /// </summary>
        /// <returns>a string representation of this XMLTag
        /// including the &lt;&gt; brackets</returns>
        public override string ToString()
        {
            return "<"
                    + (IsOpenTag ? "" : "/")
                    + (Element.Equals("!--") ? "!-- --" :
                        Element + (IsSelfCLosing && !Element.Equals("?xml") ? " /" : ""))
                    + "> ";
        }

        /// <summary>
        /// Reads a string such as "&lt;code&gt;" or "&lt;/p&gt;" and converts it 
        /// into an XMLTag, which is returned. 
        /// Throws a NullPointerException if tagText is null. 
        /// Requires tag being reasonably well-formed.
        /// </summary>
        /// <param name="tagText">tagText is a string that represents an XML
        /// including &lt; &gt; brackets 
        /// </param>
        /// <returns>an XMLTag that represents the tag given as a string</returns>
        public static XMLTag Parse(string tagText)
        {
            return NextTag(new StringBuilder(tagText));
        }

        // You don't need to call this method in your MP code
        /// <summary>
        /// Takes a string and converts it into a list of XML tokens 
        /// represented using a list of XMLTag objects.
        /// The input string represents the XML text.
        /// </summary>
        /// <param name="text">text is the input XML, and text != null</param>
        /// <returns>a list of XMLTag objects obtained from the input XML</returns>
        public static LinkedList<XMLTag> Tokenize(string text)
        {
            StringBuilder buf = new StringBuilder(text);
            LinkedList<XMLTag> queue = new LinkedList<XMLTag>();

            while (true)
            {
                XMLTag nextTag = NextTag(buf);
                if (nextTag == null)
                {
                    break;
                }
                else
                {
                    queue.AddLast(nextTag);
                }
            }

            return queue;
        }

        /// <summary>
        /// This method grabs the next XML tag from a string buffer and 
        /// returns an XMLTag object for the tag found in the buffer
        /// 
        /// It advances to next tag in input; probably not a perfect XML tag 
        /// tokenizer, but it will do for this MP
        /// </summary>
        /// <param name="buf">buf represents XML text that needs to be processed;
        /// buf is modified in this method when a tag is found
        /// and the tag and associated text are removed from buf.
        /// This effectively advances the processing point.</param>
        /// <returns>an XMLTag object for the next XML tag in buf, otherwise
        /// null if invalid</returns>
        private static XMLTag NextTag(StringBuilder buf)
        {
            int index1 = buf.ToString().IndexOf("<");
            int index2 = buf.ToString().IndexOf(">");
            bool isProlog = false;
            bool isComment = false;

            if (index1 >= 0 && index2 > index1)
            {
                // check for XML comments: <!-- -->
                if (index1 + 6 <= index2 && buf.ToString().Substring(index1 + 1, 3).Equals("!--"))
                {
                    // a comment; look for closing comment tag -->
                    index2 = buf.ToString().IndexOf("-->", index1 + 4);
                    if (index2 < 0)
                    {
                        return null;
                    }
                    else
                    {
                        buf.Insert(index1 + 4, " ");    // fixes things like <!--hi-->
                        index2 += 3;    // advance to the closing >
                        isComment = true;
                    }
                }
                // check for XML prolog: <?xml ?>
                else if (index1 + 6 <= index2 && buf.ToString().Substring(index1 + 1, 4).Equals("?xml"))
                {
                    // a prolog; look for closing comment tag ?>
                    index2 = buf.ToString().IndexOf("?>", index1 + 5);
                    if (index2 < 0)
                    {
                        return null;
                    }
                    else
                    {
                        buf.Insert(index1 + 5, " ");    // considers things like <?xml?>
                        index2 += 2;    // advance to the closing >
                        isProlog = true;
                    }
                }

                string element;
                bool isOpenTag = true;
                bool isSelfClosing = false;

                if (isProlog)
                {
                    element = "?xml";  //prolog
                    isSelfClosing = true;
                }
                else if (isComment)
                {
                    element = "!--";  // XML comments
                    isSelfClosing = true;
                }
                else
                {
                    element = buf.ToString().Substring(index1 + 1, index2 - index1 - 1).Trim();

                    // all whitespace characters; used in text parsing
                    string WHITESPACE = " \f\n\r\t";

                    // remove attributes
                    for (int i = 0; i < WHITESPACE.Length; i++)
                    {
                        int index3 = element.IndexOf(WHITESPACE[i]);
                        if (index3 >= 0)
                        {
                            element = element.Substring(0, index3);
                        }
                    }

                    // Note: an element should not be blank here (<> or </> or < />)
                    // though we are allowing an element to be blank without loss of generality

                    // determine whether opening or closing tag
                    if (element.IndexOf("/") == 0)
                    {
                        isOpenTag = false;
                        element = element.Substring(1);
                    }
                    //determine whether selfclosing
                    else if(buf[index2 - 1].Equals('/'))
                    {
                        isSelfClosing = true;
                    }

                    Regex regex = new Regex("[^a-zA-Z0-9!-]");
                    element = regex.Replace(element, "");
                }

                buf.Remove(0, index2 + 1);
                return new XMLTag(element, isOpenTag, isSelfClosing);
            }
            else
            {
                return null;
            }
        }
    }
}
