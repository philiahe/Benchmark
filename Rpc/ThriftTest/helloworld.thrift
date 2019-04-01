struct SayHelloArgs {
    1: string Name
}

struct SayHelloResultArgs {
    1: string Message
}

service Helloword {
    SayHelloResultArgs SayHello(1:SayHelloArgs request)
}