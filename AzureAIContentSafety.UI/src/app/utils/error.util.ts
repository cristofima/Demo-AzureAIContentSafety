export class ErrorUtil {
  static getErrors(errors: any): string[] {
    let keys = Object.keys(errors as { [key: string]: string[] });
    let resultArr: string[] = [];

    keys.forEach((k: string) => {
      resultArr = [...resultArr, ...errors[k]];
    });

    return resultArr;
  }
}
