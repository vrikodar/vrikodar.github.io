import socket
import threading

LISTEN_PORT = 1080   # Change this to whatever port you want


def handle_client(client_socket):
    try:
        client_socket.recv(2)
        client_socket.recv(1)  
        
        client_socket.sendall(b"\x05\x00")

        version, cmd, _, address_type = client_socket.recv(4)

        if address_type == 1:  
            address = socket.inet_ntoa(client_socket.recv(4))
        else:
            client_socket.close()
            return
        
        port = int.from_bytes(client_socket.recv(2), 'big')

        remote = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        remote.connect((address, port))

        reply = b"\x05\x00\x00\x01" + socket.inet_aton("0.0.0.0") + (0).to_bytes(2, 'big')
        client_socket.sendall(reply)

        def relay(src, dst):
            while True:
                data = src.recv(4096)
                if not data:
                    break
                dst.sendall(data)
            src.close()
            dst.close()

        threading.Thread(target=relay, args=(client_socket, remote)).start()
        threading.Thread(target=relay, args=(remote, client_socket)).start()

    except Exception as e:
        client_socket.close()


def start_socks5_server(port):
    print(f"[+] SOCKS5 proxy listening on port {port}")
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind(("0.0.0.0", port))
    server.listen(5)

    while True:
        client_socket, _ = server.accept()
        threading.Thread(target=handle_client, args=(client_socket,)).start()


if __name__ == "__main__":
    start_socks5_server(LISTEN_PORT)
