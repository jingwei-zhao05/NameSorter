namespace NameSorter {
    public interface INameComparer
    {
        int Compare(Name name1, Name name2);
    }

    public class AscendingNameComparer : INameComparer
    {
        public int Compare(Name name1, Name name2)
        {
            int result = string.Compare(name1.LastName, name2.LastName, StringComparison.OrdinalIgnoreCase);

            if (result == 0)
            {
                int givenNameCount = Math.Max(name1.GivenNames.Length, name2.GivenNames.Length);

                for (int i = 0; i < givenNameCount; i++)
                {
                    string? thisGivenName = name1.GivenNames.ElementAtOrDefault(i);
                    string? otherGivenName = name2.GivenNames.ElementAtOrDefault(i);

                    result = string.Compare(thisGivenName, otherGivenName, StringComparison.OrdinalIgnoreCase);

                    if (result != 0)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }

    public class DescendingNameComparer : INameComparer
    {
        public int Compare(Name name1, Name name2)
        {
            // To sort in descending order, just reverse the result of ascending order comparison
            return -1 * new AscendingNameComparer().Compare(name1, name2);
        }
    }

    public class Name : IComparable<Name>
    {
        public string[] GivenNames { get; }
        public string LastName { get; }

        public Name(string fullName)
        {
            var nameParts = fullName.Split(' ');
             //check number of given names
            if (nameParts.Length < 2 || nameParts.Length > 4)
            {
                throw new ArgumentException("A name must have between 2 and 4 parts: 1 or 3 given names and 1 last name.");
            }

            //extract all elements except the last one, and assign to given names
            GivenNames = nameParts[..^1];
            //assign the last element of the nameParts array to last name
            LastName = nameParts[^1];
        }

        public override string ToString()
        {
            return string.Join(" ", GivenNames) + " " + LastName;
        }

        public int CompareTo(Name? other)
        {
            return CompareTo(other, new AscendingNameComparer());
        }

        // Overloaded method to accept INameComparer for sorting
        public int CompareTo(Name? other, INameComparer comparer)
        {
            if (other is null)
            {
                return 1;
            }

            // Delegate the comparison logic to the provided comparer
            return comparer.Compare(this, other);
        }
    }
}
