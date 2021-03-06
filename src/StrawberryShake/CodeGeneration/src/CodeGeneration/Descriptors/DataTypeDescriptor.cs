using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using static StrawberryShake.CodeGeneration.NamingConventions;

namespace StrawberryShake.CodeGeneration
{
    public class DataTypeDescriptor : ICodeDescriptor
    {
        /// <summary>
        /// Describes the DataType
        /// </summary>
        /// <param name="name">
        ///
        /// </param>
        /// <param name="namespace">
        ///
        /// </param>
        /// <param name="operationTypes">
        /// The types that are subsets of the DataType represented by this descriptor.
        /// </param>
        /// <param name="implements"></param>
        /// <param name="isInterface"></param>
        public DataTypeDescriptor(
            NameString name,
            string @namespace,
            IReadOnlyList<ComplexTypeDescriptor> operationTypes,
            IReadOnlyList<string> implements,
            bool isInterface = false)
        {
            var allProperties = new Dictionary<string, PropertyDescriptor>();

            foreach (PropertyDescriptor namedTypeReferenceDescriptor in
                operationTypes.SelectMany(operationType => operationType.Properties))
            {
                if (!allProperties.ContainsKey(namedTypeReferenceDescriptor.Name))
                {
                    if (namedTypeReferenceDescriptor.Type is NonNullTypeDescriptor nonNull)
                    {
                        allProperties.Add(
                            namedTypeReferenceDescriptor.Name,
                            new PropertyDescriptor(
                                namedTypeReferenceDescriptor.Name,
                                nonNull.InnerType));
                    }
                    else
                    {
                        allProperties.Add(
                            namedTypeReferenceDescriptor.Name,
                            namedTypeReferenceDescriptor);
                    }
                }
            }

            Properties = allProperties.Select(pair => pair.Value).ToList();
            Name = name;
            RuntimeType = new(NamingConventions.CreateDataTypeName(name), @namespace);
            Implements = implements;
            IsInterface = isInterface;
        }

        /// <summary>
        /// Gets the GraphQL type name which this entity represents.
        /// </summary>
        public NameString Name { get; }
        
        /// <summary>
        /// Gets the entity name.
        /// </summary>
        public RuntimeTypeInfo RuntimeType { get; }

        /// <summary>
        /// Defines if this data type descriptor reptresents an interface.
        /// </summary>
        public bool IsInterface { get; }

        
        /// <summary>
        /// Gets the properties of this entity.
        /// </summary>
        public IReadOnlyList<PropertyDescriptor> Properties { get; }

        /// <summary>
        /// The interfaces that this data type implements. A data type does only implement
        /// interfaces, if it is part of a graphql union or interface type.
        /// </summary>
        public IReadOnlyList<string> Implements { get; }
    }
}
