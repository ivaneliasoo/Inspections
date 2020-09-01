export interface CardOption {
  name: string,
  text: string;
  helpText: string;
  icon: string | boolean;
  color: string;
  path: string | Function;
  action?: () => void | undefined;
  innerActions?: CardOptionButton[]
}

export interface CardOptionButton {
  text: string,
  color: string,
  action: Function
}