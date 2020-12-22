# Copyright 2020 FunderMaps B.V.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

""" FunderMaps Commandline Interface """

import logging
import json
import sys

import grpc

from Protos import protocol_pb2
from Protos import batch_pb2
from Protos import batch_pb2_grpc
from google.protobuf import empty_pb2

GRPC_HOST = 'localhost:5000'


def _make_protocol():
    proto_version = 0xa1
    return protocol_pb2.FunderMapsProtocol(
        version=proto_version,
        user_agent='batch_client.py')


def _print_response(response):
    if not response.task_id:
        print("Server returned an error")
    else:
        print(f"Task ID : {response.task_id}")


def bundle_create(stub, bundle):
    print("-------------- Bundle --------------")
    print(f"Bundle : {bundle}")

    request = batch_pb2.EnqueueRequest(
        protocol=_make_protocol(),
        name="bundle_building",
        payload=json.dumps({
            "BundleId": bundle,
            "Formats": [0, 1]})
    )

    response = stub.Enqueue(request)
    _print_response(response)


def bundle_batch(stub):
    print("-------------- Bundle --------------")
    print(f"Bundle : all")

    request = batch_pb2.EnqueueRequest(
        protocol=_make_protocol(),
        name="bundle_batch",
        payload=json.dumps({
            "Formats": [0, 1]})
    )

    response = stub.Enqueue(request)
    _print_response(response)


def foobar(stub):
    print("-------------- Foobar --------------")

    request = batch_pb2.EnqueueRequest(
        protocol=_make_protocol(),
        name="foobar")

    response = stub.Enqueue(request)
    _print_response(response)


def email_send(stub, recp):
    print("-------------- Email --------------")

    request = batch_pb2.EnqueueRequest(
        protocol=_make_protocol(),
        name="incident_notify",
        payload=json.dumps({
            'recipients': [recp],
        }))

    response = stub.Enqueue(request)
    _print_response(response)


def get_status(stub):
    print("-------------- Status --------------")

    response = stub.Status(empty_pb2.Empty())
    print(f"Jobs succeeded: {response.jobs_succeeded}")
    print(f"Jobs failed: {response.jobs_failed}")


def run():
    # NOTE: .close() is possible on a channel and should be used in circumstances
    #       in which the with statement does not fit the needs of the code.
    creds = grpc.ssl_channel_credentials()
    with grpc.secure_channel(GRPC_HOST, creds) as channel:
        # with grpc.insecure_channel(GRPC_HOST) as channel:
        stub = batch_pb2_grpc.BatchStub(channel)
        if len(sys.argv) == 1:
            print('Command required')
            return

        if sys.argv[1] == 'foobar':
            foobar(stub)
        elif sys.argv[1] == 'email':
            email_send(stub, sys.argv[2])
        elif sys.argv[1] == 'bundle':
            if sys.argv[2] == 'all':
                bundle_batch(stub)
            else:
                bundle_create(stub, sys.argv[2])
        elif sys.argv[1] == 'status':
            get_status(stub)
        else:
            print('Command required')


if __name__ == '__main__':
    logging.basicConfig()
    run()