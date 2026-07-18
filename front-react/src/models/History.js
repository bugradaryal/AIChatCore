export function history(
    id,
    message, 
    role, 
    date) {
  return {
    id,
    message,
    role,
    date: new Date(date),   // JS Date objesine çeviriyoruz
  };
}