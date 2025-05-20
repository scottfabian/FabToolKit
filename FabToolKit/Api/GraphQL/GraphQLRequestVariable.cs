namespace FabToolKit.Api.GraphQL;

public class GraphQLRequestVariables : Dictionary<string, object>
{
    public GraphQLRequestVariables(){}

    public GraphQLRequestVariables AddSimpleVariable(string keyName, string keyValue)
    {
        this[keyName] = keyValue;
        return this;
    }

    public GraphQLRequestVariables AddNestedVariable(string rootName, string keyName, object keyValue)
    {
        if (this.ContainsKey(rootName))
        {
            if (this[rootName].GetType() == typeof(Dictionary<string, object>))
            {
                Dictionary<string, object> rootValue = (Dictionary<string, object>)this[rootName];
                rootValue[keyName] = keyValue;
                return this;
            }

        }

        this[rootName] = new Dictionary<string, object>() { { keyName, keyValue } };
        return this;
    }
}
