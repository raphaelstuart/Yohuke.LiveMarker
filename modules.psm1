function RunProcess($process, $arg)
{
    if ($null -ne $arg)
    {
        $proc = Start-Process $process -NoNewWindow -PassThru -ArgumentList $arg
    }
    else
    {
        $proc = Start-Process $process -NoNewWindow -PassThru
    }

    #cache for handle
    $handle = $proc.Handle
    $proc.WaitForExit()

    if ($proc.ExitCode -ne 0)
    {
        Write-OutPut "+++++++++++++++++++++++++++++++++++++"
        Write-OutPut "+  Action failed...                 +"
        Write-OutPut "+++++++++++++++++++++++++++++++++++++"
        throw "Error when build application."
    }
}