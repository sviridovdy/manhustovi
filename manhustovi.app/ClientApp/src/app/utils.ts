export class Utils {
  public static getWeekDayString(timestamp: number): string {
    let dayNames = ["Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя"];
    let date = new Date(timestamp * 1000);
    return dayNames[date.getDay()];
  }

  public static getDateString(timestamp: number): string {
    let monthNames = ["Січня", "Лютого", "Марцє", "Цьвїтнє", "Має", "Червнє", "Липнє", "Серпня", "Вересня", "Жовтнє", "Падолиста", "Груднє"];
    let date = new Date(timestamp * 1000);
    let string = `${date.getDate()} ${monthNames[date.getMonth()]}`;
    let year = date.getFullYear();
    if (new Date().getFullYear() != year) {
      string = string + ` ${year}`;
    }
    return string;
  }
}
