import * as signalR from "@microsoft/signalr";
import { ElNotification } from "element-plus";
import { useUserStoreHook } from "@/store/modules/user";

export function useSignalR() {
  const { VITE_BASE_URL } = import.meta.env;
  const connection = new signalR.HubConnectionBuilder()
    .withUrl(VITE_BASE_URL + "/noticeHub")
    .withAutomaticReconnect()
    .build();
  connection.on("ReceiveMessage", function (title, content) {
    ElNotification.info({
      title: title,
      message: content,
      showClose: false,
      position: "bottom-right"
    });
  });

  function initialization() {
    connection.start().catch(err => {
      console.error(err);
    });
    setTimeout(() => {
      sendMessage();
    }, 2000);
  }

  function sendMessage() {
    connection
      .invoke("SendMessage", useUserStoreHook()?.username ?? "")
      .then(() => {
        setTimeout(() => {
          sendMessage();
        }, 60000);
      })
      .catch(function (err) {
        console.error(err.toString());
      });
  }

  return {
    initialization
  };
}
