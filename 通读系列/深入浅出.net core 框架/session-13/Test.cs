class Test
{
    static void Run() { 
    }

    static int TaskMethod(int count, CancellationToken token){
        for (int i = 0; i < count ; i++) {
            Thread.Sleep(1000);
            if(token.IsCancellationRequested) return -1;
        }
        
        return count;
    }
}