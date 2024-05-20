
namespace SmartLogger.Infra;

public delegate Output AsyncMethodCaller<Input,Output>(Input inputParam);
public delegate void AsyncMethodCaller<Input>(Input inputParam);
public delegate void AsyncMethodCaller();
