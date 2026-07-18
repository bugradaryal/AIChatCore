export function session(
    id, 
    title, 
    created_date) {
  return {
    id,
    title,
    //createdDate: new Date(created_date),   // JS Date objesine çeviriyoruz
  };
}