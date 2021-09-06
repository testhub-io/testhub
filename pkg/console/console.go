package console

import (
	"fmt"
	"github.com/fatih/color"
)

func PrintError(format string, a ...interface{}) {
	color.Red(format+"\n", a...)
}

func PrintLn(format string, a ...interface{}) {
	fmt.Println(fmt.Sprintf(format, a...))
}

func PrintGreen(format string, a ...interface{}) {
	color.Green(format+"\n", a...)
}
